using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterShip;

public partial class ShipDataRecord : ObservableObject
{
	public IShipDataMaster Ship { get; }
	public DialogAlbumMasterShipTranslationViewModel DialogAlbumMasterShip { get; }

	public bool IsPlayerShip => !Ship.IsAbyssalShip;
	public bool IsAbyssalShip => Ship.IsAbyssalShip;

	public int Level { get; set; } = 1;

	public SolidColorBrush NameColor => (Ship.GetShipNameColor() switch
	{
		{ } c when c == System.Drawing.SystemColors.ControlText => Configuration.Config.UI.ForeColor,
		{ } c => c,
	}).ToBrush();

	public string ResourceNameText => $"{Ship.ResourceName} {Ship.ResourceGraphicVersion}/{Ship.ResourceVoiceVersion}/{Ship.ResourcePortVoiceVersion}";
	public string ResourceNameToolTip => string.Format(AlbumMasterShipResources.ResourceNameToolTip,
		Ship.ResourceName, Ship.ResourceGraphicVersion, Ship.ResourceVoiceVersion, Ship.ResourcePortVoiceVersion, Constants.GetVoiceFlag(Ship.VoiceFlag));

	public string ShipBannerToolTip => DialogAlbumMasterShip.ShipBannerToolTip;

	public string ShipNameToolTip => Ship.IsAbyssalShip switch
	{
		true => "",
		_ => Ship.NameReading + "\r\n",
	} + DialogAlbumMasterShip.RightClickToCopy;

	#region ShipType

	public string ShipType => ShipTypePrefix + " " + Ship.IsLandBase switch
	{
		true => DialogAlbumMasterShip.Installation,
		_ => Ship.ShipTypeName
	};

	public string ShipTypeToolTip => ShipTypeToolTipHeader + "\n\n" +
									 $"{DialogAlbumMasterShip.Equippable}:\n" +
									 EquippableString;

	private string ShipTypeToolTipHeader => Ship.IsAbyssalShip switch
	{
		true => $"{DialogAlbumMasterShip.ShipClassId}: {Ship.ShipClass}",
		_ when !IsShipClassKnown => $"{DialogAlbumMasterShip.ShipClassUnknown}: {Ship.ShipClass}",
		_ => $"{ShipClassName}: {Ship.ShipClass}"
	};

	private string EquippableString => string.Join("\r\n", Ship.EquippableCategories.Select(id => KCDatabase.Instance.EquipmentTypes[id].NameEN)
		.Concat(KCDatabase.Instance.MasterEquipments.Values.Where(eq => eq.EquippableShipsAtExpansion.Contains(Ship.ShipId)).Select(eq => eq.NameEN + $" ({DialogAlbumMasterShip.ReinforcementSlot})")));

	private string ShipTypePrefix => Ship.IsAbyssalShip switch
	{
		true => "深海",
		_ when IsShipClassKnown => ShipClassName,
		_ => ""
	};

	private bool IsShipClassKnown => ShipClassName != "不明";
	private string ShipClassName => Constants.GetShipClass(Ship.ShipClass, Ship.ShipId);
	#endregion

	#region Stats

	public string HpMin => Ship.IsAbyssalShip switch
	{
		true => AbyssalShipHp,
		_ => Ship.HPMin.ToString()
	};
	public string? HpMinToolTip => Ship.IsAbyssalShip switch
	{
		true => null,
		_ => string.Format(DialogAlbumMasterShip.HpMinToolTip, Ship.HPMaxModernized, Ship.HPMaxModernizable)
	};
	public string HpMax => Ship.IsAbyssalShip switch
	{
		true => AbyssalShipHp,
		_ => Ship.HPMaxMarried.ToString()
	};
	public string? HpMaxToolTip => Ship.IsAbyssalShip switch
	{
		true => null,
		_ => string.Format(DialogAlbumMasterShip.HpMaxToolTip,
			Ship.HPMaxMarriedModernized, Ship.HPMaxMarriedModernizable, Ship.HPMax)
	};

	private string AbyssalShipHp => Ship.HPMin switch
	{
		> 0 => Ship.HPMin.ToString(),
		_ => "???"
	};

	public string FirepowerMin => Ship.IsAbyssalShip switch
	{
		true => Ship.FirepowerMax.ToString(),
		_ => Ship.FirepowerMin.ToString()
	};
	public string FirepowerMax => Ship.IsAbyssalShip switch
	{
		true => (Ship.FirepowerMax + Slots
				.Select(s => s.Equipment)
				.Sum(e => e?.Firepower ?? 0))
			.ToString(),

		_ => Ship.FirepowerMax.ToString()
	};

	public string TorpedoMin => Ship.IsAbyssalShip switch
	{
		true => Ship.TorpedoMax.ToString(),
		_ => Ship.TorpedoMin.ToString()
	};
	public string TorpedoMax => Ship.IsAbyssalShip switch
	{
		true => (Ship.TorpedoMax + Slots
				.Select(s => s.Equipment)
				.Sum(e => e?.Torpedo ?? 0))
			.ToString(),

		_ => Ship.TorpedoMax.ToString()
	};

	public string AaMin => Ship.IsAbyssalShip switch
	{
		true => Ship.AAMax.ToString(),
		_ => Ship.AAMin.ToString()
	};
	public string AaMax => Ship.IsAbyssalShip switch
	{
		true => (Ship.AAMax + Slots
				.Select(s => s.Equipment)
				.Sum(e => e?.AA ?? 0))
			.ToString(),

		_ => Ship.AAMax.ToString()
	};

	public string ArmorMin => Ship.IsAbyssalShip switch
	{
		true => Ship.ArmorMax.ToString(),
		_ => Ship.ArmorMin.ToString()
	};
	public string ArmorMax => Ship.IsAbyssalShip switch
	{
		true => (Ship.ArmorMax + Slots
				.Select(s => s.Equipment)
				.Sum(e => e?.Armor ?? 0))
			.ToString(),

		_ => Ship.ArmorMax.ToString()
	};

	public string AswMin => Ship.IsAbyssalShip switch
	{
		true => Ship.ASW?.IsDetermined switch
		{
			true => Ship.ASW.Maximum.ToString(),
			_ => "???"
		},
		_ => GetParameterMinBound(Ship.ASW)
	};
	public string AswMax => Ship.IsAbyssalShip switch
	{
		true => Ship.ASW?.IsDetermined switch
		{
			true => (Ship.ASW.Maximum + EquipmentASW).ToString(),
			_ => EquipmentASW.ToString("+0;-0")
		},
		_ => GetParameterMax(Ship.ASW)
	};

	private int EquipmentASW => Slots
		.Select(s => s.Equipment)
		.Sum(e => e?.ASW ?? 0);

	public string EvasionMin => Ship.IsAbyssalShip switch
	{
		true => Ship.Evasion?.IsDetermined switch
		{
			true => Ship.Evasion.Maximum.ToString(),
			_ => "???"
		},
		_ => GetParameterMinBound(Ship.Evasion)
	};
	public string EvasionMax => Ship.IsAbyssalShip switch
	{
		true => Ship.Evasion?.IsDetermined switch
		{
			true => (Ship.Evasion.Maximum + EquipmentEvasion).ToString(),
			_ => EquipmentEvasion.ToString("+0;-0")
		},
		_ => GetParameterMax(Ship.Evasion)
	};

	private int EquipmentEvasion => Slots
		.Select(s => s.Equipment)
		.Sum(e => e?.Evasion ?? 0);

	public string LosMin => Ship.IsAbyssalShip switch
	{
		true => Ship.LOS?.IsDetermined switch
		{
			true => Ship.LOS.Maximum.ToString(),
			_ => "???"
		},
		_ => GetParameterMinBound(Ship.LOS)
	};
	public string LosMax => Ship.IsAbyssalShip switch
	{
		true => Ship.LOS?.IsDetermined switch
		{
			true => (Ship.LOS.Maximum + EquipmentLoS).ToString(),
			_ => EquipmentLoS.ToString("+0;-0")
		},
		_ => GetParameterMax(Ship.LOS)
	};

	private int EquipmentLoS => Slots
		.Select(s => s.Equipment)
		.Sum(e => e?.LOS ?? 0);

	public string LuckMin => Ship.IsAbyssalShip switch
	{
		true => Ship.LuckMax switch
		{
			> 0 => Ship.LuckMax.ToString(),
			_ => "???"
		},
		_ => Ship.LuckMin.ToString()
	};
	public string LuckMax => Ship.IsAbyssalShip switch
	{
		true => (Ship.LuckMax + EquipmentLuck) switch
		{
			int luck and > 0 => luck.ToString(),
			_ => "???"
		},
		_ => Ship.LuckMax.ToString()
	};

	private int EquipmentLuck => Slots
		.Select(s => s.Equipment)
		.Sum(e => e?.Luck ?? 0);

	public string AccuracyMin => Ship.LuckMin switch
	{
		> 0 => Accuracy(DefaultLevel, Ship.LuckMin).ToString("N0"),
		_ => "???"
	};

	public string AccuracyMax => Ship.IsAbyssalShip switch
	{
		true => Ship.LuckMin switch
		{
			> 0 => (Accuracy(DefaultLevel, Ship.LuckMin) + EquipmentAccuracy).ToString("N0"),
			_ => EquipmentAccuracy.ToString("+0;-0")
		},

		_ => Ship.LuckMin switch
		{
			> 0 => Accuracy(99, Ship.LuckMin).ToString("N0"),
			_ => "???"
		}
	};

	private static double Accuracy(int level, int luck) => 2 * Math.Sqrt(level) + 1.5 * Math.Sqrt(luck);
	private int DefaultLevel => (Ship.IsAbyssalShip && Ship.IsSubmarine) switch
	{
		true => 50,
		_ => 1
	};

	private int EquipmentAccuracy => Slots
		.Select(s => s.Equipment)
		.Sum(e => e?.Accuracy ?? 0);

	public string AswCurrent => Ship.ASW?.IsDetermined switch
	{
		true => Ship.ASW.GetParameter(Level).ToString(),
		_ => ""
	};
	public string EvasionCurrent => Ship.Evasion?.IsDetermined switch
	{
		true => Ship.Evasion.GetParameter(Level).ToString(),
		_ => ""
	};
	public string LosCurrent => Ship.LOS?.IsDetermined switch
	{
		true => Ship.LOS.GetParameter(Level).ToString(),
		_ => ""
	};
	public string AccuracyCurrent => Ship.LuckMin switch
	{
		> 0 => ((int)Accuracy(Level, Ship.LuckMin)).ToString("N0"),
		_ => ""
	};

	public static string GetParameterMinBound(IParameter? param)
	{

		if (param == null || param.MinimumEstMax == ShipParameterRecord.Parameter.MaximumDefault)
			return "???";
		if (param.MinimumEstMin == param.MinimumEstMax)
			return param.MinimumEstMin.ToString();
		if (param.MinimumEstMin == ShipParameterRecord.Parameter.MinimumDefault && param.MinimumEstMax == param.Maximum)
			return "???";
		return $"{param.MinimumEstMin}～{param.MinimumEstMax}";
	}

	public static string GetParameterMax(IParameter? param)
	{

		if (param == null || param.Maximum == ShipParameterRecord.Parameter.MaximumDefault)
			return "???";
		return param.Maximum.ToString();

	}

	#endregion

	#region SubStats

	public int Speed => Ship.Speed;

	public int Range => Ship.IsAbyssalShip switch
	{
		true => Math.Max(Ship.Range, EquipmentRangeMax),
		_ => Ship.Range
	};
	public string? RangeToolTip => Ship.IsAbyssalShip switch
	{
		true => $"{DialogAlbumMasterShip.DefaultRange}: {Constants.GetRange(Ship.Range)}",
		_ => null
	};

	private int EquipmentRangeMax => Slots.Select(s => s.Equipment) switch
	{
		{ } equipment when equipment.Any() => equipment.Max(e => e?.Range ?? 0),
		_ => 0
	};

	public int Fuel => Ship.Fuel;
	public int Ammo => Ship.Ammo;
	public string ConsumptionToolTip => string.Format(DialogAlbumMasterShip.RepairTooltip,
		(Ship.Fuel * 0.06),
		(Ship.Fuel * 0.032),
		(int)(Ship.Fuel * 0.06 * (Ship.HPMaxMarried - 1)),
		(int)(Ship.Fuel * 0.032 * (Ship.HPMaxMarried - 1))
	);

	private DisplayedMessage DisplayedMessage { get; set; }

	public string Message => DisplayedMessage switch
	{
		DisplayedMessage.Album => Ship.MessageAlbum,
		_ => Ship.MessageGet
	};

	#endregion

	#region construction, scrap, modernization

	public string BuildingTime => DateTimeHelper.ToTimeRemainString(new TimeSpan(0, Ship.BuildingTime, 0));

	#endregion

	// shows remodel for player boats and air, day attack and night attack for abyssals
	#region RemodelOrAttackTypes

	private IShipDataMaster? RemodelBeforeShip => Ship.RemodelBeforeShip;

	public string RemodelBeforeShipName => RemodelBeforeShip?.NameEN ?? DialogAlbumMasterShip.Empty;
	public string? RemodelBeforeShipNameToolTip => RemodelBeforeShip switch
	{
		{ } => DialogAlbumMasterShip.RemodelBeforeShipNameToolTip,
		_ => null
	};

	public string? RemodelBeforeLevel => RemodelBeforeShip switch
	{
		{ } => string.Format("Lv. {0}", RemodelBeforeShip.RemodelAfterLevel),
		_ => null
	};

	public IEnumerable<RemodelItem> RemodelBeforeItems => RemodelItems(RemodelBeforeShip);
	public string? RemodelBeforeItemsToolTip => RemodelBeforeShip switch
	{
		null => null,

		_ => string.Join("\n", RemodelItems(RemodelBeforeShip)
			.Select(r => $"{RemodelItemName(r.Icon)}: {r.Count}"))
	};

	public string RemodelBeforeAmmo => RemodelBeforeShip?.RemodelAmmo.ToString() ?? "-";
	public string RemodelBeforeSteel => RemodelBeforeShip?.RemodelSteel.ToString() ?? "-";

	private IShipDataMaster? RemodelAfterShip => Ship.RemodelAfterShip switch
	{
		null => null,
		_ => Ship
	};

	public string RemodelAfterShipName => RemodelAfterShip?.RemodelAfterShip?.NameEN ?? DialogAlbumMasterShip.Empty;
	public string? RemodelAfterShipNameToolTip => RemodelAfterShip switch
	{
		{ } => DialogAlbumMasterShip.RemodelBeforeShipNameToolTip,
		_ => null
	};

	public string? RemodelAfterLevel => RemodelAfterShip switch
	{
		{ } => string.Format("Lv. {0}", RemodelAfterShip.RemodelAfterLevel),
		_ => null
	};

	public IEnumerable<RemodelItem> RemodelAfterItems => RemodelItems(RemodelAfterShip);
	public string? RemodelAfterItemsToolTip => RemodelAfterShip switch
	{
		null => null,

		_ => string.Join("\n", RemodelItems(RemodelAfterShip)
			.Select(r => $"{RemodelItemName(r.Icon)}: {r.Count}"))
	};

	public string RemodelAfterAmmo => RemodelAfterShip?.RemodelAmmo.ToString() ?? "-";
	public string RemodelAfterSteel => RemodelAfterShip?.RemodelSteel.ToString() ?? "-";

	private static IEnumerable<RemodelItem> RemodelItems(IShipDataMaster? ship) => ship switch
	{
		null => Enumerable.Empty<RemodelItem>(),

		_ => new List<RemodelItem>
			{
				new(IconContent.ItemCatapult, ship.NeedCatapult),
				new(IconContent.ItemActionReport, ship.NeedActionReport),
				new(IconContent.ItemBlueprint, ship.NeedBlueprint),
				new(IconContent.ItemAviationMaterial, ship.NeedAviationMaterial),
			}
			.Where(r => r.Count > 0)
	};

	private string RemodelItemName(IconContent icon) => icon switch
	{
		IconContent.ItemBlueprint => EncycloRes.Blueprint,
		IconContent.ItemCatapult => EncycloRes.PrototypeCatapult,
		IconContent.ItemActionReport => DialogAlbumMasterShip.ActionReport,
		IconContent.ItemAviationMaterial => DialogAlbumMasterShip.AviationMaterial,

		_ => "???"
	};

	public string AirPower => Calculator.GetAirSuperiority(Ship).ToString();
	public string DayAttack => Core.Types.Attacks.DayAttack.AttackDisplay(Calculator.GetDayAttackKind(Ship.DefaultSlot?.ToArray(), Ship.ShipID, -1));
	public string NightAttack => Core.Types.Attacks.NightAttack.AttackDisplay(Calculator.GetNightAttackKind(Ship.DefaultSlot?.ToArray(), Ship.ShipID, -1));

	#endregion

	public IconContent RarityIcon => Ship.Rarity switch
	{
		0 => IconContent.RarityRed,
		1 => IconContent.RarityBlueC,
		2 => IconContent.RarityBlueB,
		3 => IconContent.RarityBlueA,
		4 => IconContent.RaritySilver,
		5 => IconContent.RarityGold,
		6 => IconContent.RarityHoloB,
		7 => IconContent.RarityHoloA,
		8 => IconContent.RarityCherry,

		_ => IconContent.RarityRed
	};

	// default slot is null for unknown boats
	public IEnumerable<EquipmentSlot> Slots => Ship.DefaultSlot switch
	{
		{ } equipmentIds => equipmentIds
			.Take(Ship.SlotSize)
			.Select(i => i switch
			{
				-1 => null,
				_ => KCDatabase.Instance.MasterEquipments[i]
			})
			.Zip(Ship.Aircraft, (e, s) => new EquipmentSlot(s, e, EquipmentStatus.Known)),

		null => Enumerable.Repeat((IEquipmentDataMaster?)null, Ship.SlotSize)
			.Zip(Ship.Aircraft, (e, s) => new EquipmentSlot(s, e, EquipmentStatus.Unknown)),
	};

	public ShipDataRecord(IShipDataMaster ship)
	{
		Ship = ship;
		DialogAlbumMasterShip = Ioc.Default.GetService<DialogAlbumMasterShipTranslationViewModel>()!;

		DisplayedMessage = Ship.MessageAlbum switch
		{
			null or "" => DisplayedMessage.Get,
			_ => DisplayedMessage.Album
		};
	}

	[RelayCommand]
	private void SwitchMessage()
	{
		DisplayedMessage = DisplayedMessage switch
		{
			DisplayedMessage.Get when Ship.MessageAlbum is not null or "" => DisplayedMessage.Album,
			_ => DisplayedMessage.Get
		};
	}
}
