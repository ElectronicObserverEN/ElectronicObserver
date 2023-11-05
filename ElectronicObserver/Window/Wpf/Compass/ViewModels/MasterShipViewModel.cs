using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Tools.DialogAlbumMasterShip;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Wpf.Compass.ViewModels;

public partial class MasterShipViewModel : ObservableObject
{
	public IShipDataMaster? Ship { get; set; }

	public IEnumerable<MasterShipSlotViewModel> Slots => Slot?.Select(id => id switch 
		{
			> 0 => KCDatabase.Instance.MasterEquipments[id],
			_ => null,
		})
		.Zip(Ship?.Aircraft ?? Enumerable.Empty<int>(), (equip, size) => (Equipment: equip, Size: size))
		.Select(s => new MasterShipSlotViewModel
		{
			Equipment = s.Equipment,
			Size = s.Size,
		})
		.Take(Math.Max(Slot?.Count(id => id > 0) ?? 0, Ship?.SlotSize ?? 0)) 
	?? Enumerable.Empty<MasterShipSlotViewModel>();

	public int[]? Slot { get; set; }
	public int Level { get; set; } = -1;
	public int Hp { get; set; }
	public int Firepower { get; set; }
	public int Torpedo { get; set; }
	public int Aa { get; set; }
	public int Armor { get; set; }

	public int ShipId => Ship?.ShipID ?? -1;
	public string Name => IsUnknown switch
	{
		true => "???",
		_ => Ship?.NameEN ?? "-"
	};
	public int MaxNameWidth { get; set; }
	public string? NameToolTip => Ship switch
	{
		{ IsAbyssalShip: true } => GetShipString(Ship.ShipID, Ship.DefaultSlot.ToArray(), Level),
		{ IsAbyssalShip: false } => GetShipString(Ship.ShipID, Slot, Level, Hp, Firepower, Torpedo, Aa, Armor),
		_ => null
	};

	public string? EquipmentToolTip => Ship switch
	{
		not null => GetEquipmentString(Ship.ShipID, Slot),
		_ => null
	};

	public SolidColorBrush ShipNameBrush => Ship switch
	{
		{ IsAbyssalShip: true } => Ship.GetShipNameColor().ToBrush(),
		_ => Utility.Configuration.Config.UI.ForeColor.ToBrush()
	};

	public bool IsUnknown { get; init; }

	public MasterShipViewModel()
	{
		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		ConfigurationChanged();
	}

	private void ConfigurationChanged()
	{
		MaxNameWidth = Utility.Configuration.Config.FormCompass.MaxShipNameWidth;
	}

	private bool CanOpenShipEncyclopedia(int id) => id > 0;

	[RelayCommand(CanExecute = nameof(CanOpenShipEncyclopedia))]
	private void OpenShipEncyclopedia(int shipId)
	{
		new DialogAlbumMasterShipWpf(shipId).Show(App.Current.MainWindow);
	}

	private static string? GetShipString(int shipID, int[] slot, int level)
	{

		IShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
		if (ship == null) return null;

		return GetShipString(shipID, slot, level, ship.HPMin, ship.FirepowerMax, ship.TorpedoMax, ship.AAMax, ship.ArmorMax,
			ship.ASW != null && !ship.ASW.IsMaximumDefault ? ship.ASW.Maximum : -1,
			ship.Evasion != null && !ship.Evasion.IsMaximumDefault ? ship.Evasion.Maximum : -1,
			ship.LOS != null && !ship.LOS.IsMaximumDefault ? ship.LOS.Maximum : -1,
			ship.LuckMin);
	}

	private static string? GetShipString(int shipID, int[]? slot, int level, int hp, int firepower, int torpedo, int aa, int armor)
	{
		IShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
		if (ship == null) return null;

		return GetShipString(shipID, slot, level, hp, firepower, torpedo, aa, armor,
			ship.ASW != null && ship.ASW.IsAvailable ? ship.ASW.GetParameter(level) : -1,
			ship.Evasion != null && ship.Evasion.IsAvailable ? ship.Evasion.GetParameter(level) : -1,
			ship.LOS != null && ship.LOS.IsAvailable ? ship.LOS.GetParameter(level) : -1,
			level > 99 ? Math.Min(ship.LuckMin + 3, ship.LuckMax) : ship.LuckMin);
	}

	private static string? GetShipString(int shipID, int[]? slot, int level, int hp, int firepower, int torpedo, int aa, int armor, int asw, int evasion, int los, int luck)
	{

		IShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];
		if (ship == null) return null;

		int firepower_c = firepower;
		int torpedo_c = torpedo;
		int aa_c = aa;
		int armor_c = armor;
		int asw_c = asw;
		int evasion_c = evasion;
		int los_c = los;
		int luck_c = luck;
		int range = ship.Range;

		asw = Math.Max(asw, 0);
		evasion = Math.Max(evasion, 0);
		los = Math.Max(los, 0);

		if (slot != null)
		{
			int count = slot.Length;
			for (int i = 0; i < count; i++)
			{
				IEquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments[slot[i]];
				if (eq == null) continue;

				firepower += eq.Firepower;
				torpedo += eq.Torpedo;
				aa += eq.AA;
				armor += eq.Armor;
				asw += eq.ASW;
				evasion += eq.Evasion;
				los += eq.LOS;
				luck += eq.Luck;
				range = Math.Max(range, eq.Range);
			}
		}


		StringBuilder? sb = new();

		sb.Append(ship.ShipTypeName).Append(" ").AppendLine(ship.NameWithClass);
		if (level > 0)
			sb.Append("Lv. ").Append(level.ToString());
		sb.Append(" (ID: ").Append(shipID).AppendLine(")");

		sb.Append(EncycloRes.HP + ": ").Append(hp).AppendLine();

		sb.Append(GeneralRes.Firepower + ": ").Append(firepower_c);
		if (firepower_c != firepower)
			sb.Append("/").Append(firepower);
		sb.AppendLine();

		sb.Append(GeneralRes.Torpedo + ": ").Append(torpedo_c);
		if (torpedo_c != torpedo)
			sb.Append("/").Append(torpedo);
		sb.AppendLine();

		sb.Append(GeneralRes.AntiAir + ": ").Append(aa_c);
		if (aa_c != aa)
			sb.Append("/").Append(aa);
		sb.AppendLine();

		sb.Append(GeneralRes.Armor + ": ").Append(armor_c);
		if (armor_c != armor)
			sb.Append("/").Append(armor);
		sb.AppendLine();

		sb.Append(GeneralRes.ASW + ": ");
		if (asw_c < 0) sb.Append("???");
		else sb.Append(asw_c);
		if (asw_c != asw)
			sb.Append("/").Append(asw);
		sb.AppendLine();

		sb.Append(GeneralRes.Evasion + ": ");
		if (evasion_c < 0) sb.Append("???");
		else sb.Append(evasion_c);
		if (evasion_c != evasion)
			sb.Append("/").Append(evasion);
		sb.AppendLine();

		sb.Append(GeneralRes.LoS + ": ");
		if (los_c < 0) sb.Append("???");
		else sb.Append(los_c);
		if (los_c != los)
			sb.Append("/").Append(los);
		sb.AppendLine();

		sb.Append(GeneralRes.Luck + ": ").Append(luck_c);
		if (luck_c != luck)
			sb.Append("/").Append(luck);
		sb.AppendLine();

		sb.AppendFormat(GeneralRes.Range + ": {0} / " + GeneralRes.Speed + ": {1}\r\n" + GeneralRes.Encyclopedia + "\r\n",
			Constants.GetRange(range),
			Constants.GetSpeed(ship.Speed));

		return sb.ToString();

	}

	private static string? GetEquipmentString(int shipID, int[]? slot)
	{
		StringBuilder sb = new();
		IShipDataMaster ship = KCDatabase.Instance.MasterShips[shipID];

		if (ship == null || slot == null) return null;

		for (int i = 0; i < slot.Length; i++)
		{
			var eq = KCDatabase.Instance.MasterEquipments[slot[i]];
			if (eq != null)
				sb.AppendFormat("[{0}] {1}\r\n", ship.Aircraft[i], eq.NameEN);
		}

		sb.AppendFormat("\r\n" + GeneralRes.DayBattle + ": {0}\r\n" + GeneralRes.NightBattle + ": {1}\r\n",
			DayAttack.AttackDisplay(Calculator.GetDayAttackKind(slot, ship.ShipID, -1)),
			NightAttack.AttackDisplay(Calculator.GetNightAttackKind(slot, ship.ShipID, -1)));

		/*
		 todo: I don't think this matters in compass
		if it does, we either need an overload that can handle this or make a mock ShipData
		{
			int aacutin = Calculator.GetAACutinKind(shipID, slot);
			if (aacutin != 0)
			{
				sb.AppendFormat(GeneralRes.AntiAir + ": {0}\r\n", AntiAirCutIn.FromId(aacutin).EquipmentConditionsSingleLineDisplay());
			}
		}
		*/
		{
			int airsup = Calculator.GetAirSuperiority(slot, ship.Aircraft.ToArray());
			if (airsup > 0)
			{
				sb.AppendFormat(GeneralRes.AirPower + ": {0}\r\n", airsup);
			}
		}

		return sb.ToString();
	}
}
