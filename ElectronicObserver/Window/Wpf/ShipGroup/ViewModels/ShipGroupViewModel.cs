using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.ShipGroup.ViewModels;

public record ShipGroupItemViewModel(IShipData Ship)
{
	public int MasterId => Ship.MasterID;
	public string ShipTypeName => Ship.MasterShip.ShipTypeName;
	public string Name => Ship.Name;
	public int Level => Ship.Level;
	public int ExpTotal => Ship.ExpTotal;
	public int ExpNext => Ship.ExpNext;
	public int ExpRemodel => Ship.ExpNextRemodel;
	public string Hp => $"{Ship.HPCurrent}/{Ship.HPMax}";
	public int Condition => Ship.Condition;
	public int Fuel => Ship.Fuel;
	public int Ammo => Ship.Ammo;

	public IEquipmentData? Slot1 => Ship.AllSlotInstance[0];
	public IEquipmentData? Slot2 => Ship.AllSlotInstance[1];
	public IEquipmentData? Slot3 => Ship.AllSlotInstance[2];
	public IEquipmentData? Slot4 => Ship.AllSlotInstance[3];
	public IEquipmentData? Slot5 => Ship.AllSlotInstance[4];
	public string ExpansionSlot => Ship.ExpansionSlotInstance?.NameWithLevel ?? "";

	public string Aircraft1 => $"{Ship.Aircraft[0]}/{Ship.MasterShip.Aircraft[0]}";
	public string Aircraft2 => $"{Ship.Aircraft[1]}/{Ship.MasterShip.Aircraft[1]}";
	public string Aircraft3 => $"{Ship.Aircraft[2]}/{Ship.MasterShip.Aircraft[2]}";
	public string Aircraft4 => $"{Ship.Aircraft[3]}/{Ship.MasterShip.Aircraft[3]}";
	public string Aircraft5 => $"{Ship.Aircraft[4]}/{Ship.MasterShip.Aircraft[4]}";
	public string AircraftTotal => $"{Ship.AircraftTotal}/{Ship.MasterShip.AircraftTotal}";

	public string Fleet => Ship.FleetWithIndex;
	public int RepairTime => Ship.RepairTime;
	public int RepairSteel => Ship.RepairSteel;
	public int RepairFuel => Ship.RepairFuel;

	public int Firepower => Ship.FirepowerBase;
	public int FirepowerRemain => Ship.FirepowerRemain;
	public int FirepowerTotal => Ship.FirepowerTotal;

	public int Torpedo => Ship.TorpedoBase;
	public int TorpedoRemain => Ship.TorpedoRemain;
	public int TorpedoTotal => Ship.TorpedoTotal;

	public int AA => Ship.AABase;
	public int AARemain => Ship.AARemain;
	public int AATotal => Ship.AATotal;

	public int Armor => Ship.ArmorBase;
	public int ArmorRemain => Ship.ArmorRemain;
	public int ArmorTotal => Ship.ArmorTotal;

	public int ASW => Ship.ASWBase;
	public int ASWTotal => Ship.ASWTotal;

	public int Evasion => Ship.EvasionBase;
	public int EvasionTotal => Ship.EvasionTotal;

	public int LOS => Ship.LOSBase;
	public int LOSTotal => Ship.LOSTotal;

	public int Luck => Ship.LuckBase;
	public int LuckRemain => Ship.LuckRemain;
	public int LuckTotal => Ship.LuckTotal;

	public int BomberTotal => Ship.BomberTotal;
	public int Speed => Ship.Speed;
	public int Range => Ship.Range;

	public int AirBattlePower => Ship.AirBattlePower;
	public int ShellingPower => Ship.ShellingPower;
	public int AircraftPower => Ship.AircraftPower;
	public int AntiSubmarinePower => Ship.AntiSubmarinePower;
	public int TorpedoPower => Ship.TorpedoPower;
	public int NightBattlePower => Ship.NightBattlePower;

	public bool Locked => Ship.IsLocked;
	public int SallyArea => Ship.SallyArea;
}

public partial class ShipGroupViewModel : AnchorableViewModel
{
	private KCDatabase Db { get; }

	public FormShipGroupTranslationViewModel FormShipGroup { get; }

	public ObservableCollection<ShipGroupItemViewModel> Ships { get; set; } = new();
	public ObservableCollection<ShipGroupData> Groups { get; }
	public ShipGroupData? SelectedGroup { get; set; }
	
	public List<ShipGroupItemViewModel> SelectedShips { get; set; } = new();

	public string StatusBarText => MakeStatusBarText(SelectedGroup, SelectedShips);

	private string MakeStatusBarText(ShipGroupData? group, List<ShipGroupItemViewModel> selectedShips)
	{
		if (group is null) return "";

		int membersCount = group.MembersInstance.Count(s => s != null);
		int levelsum = group.MembersInstance.Sum(s => s?.Level ?? 0);
		double levelAverage = levelsum / Math.Max(membersCount, 1.0);
		int expsum = group.MembersInstance.Sum(s => s?.ExpTotal ?? 0);
		double expAverage = expsum / Math.Max(membersCount, 1.0);

		string statusBarText = "";

		if (selectedShips.Count > 1)
		{
			int selectedShipCount = selectedShips.Count;
			int totalShipCount = group.Members.Count;
			IEnumerable<int> levels = selectedShips.Select(s => s.Level);
			IEnumerable<int> exp = selectedShips.Select(s => s.ExpTotal);

			statusBarText += string.Format(ShipGroupResources.SelectedShips, selectedShipCount, totalShipCount);
			statusBarText += string.Format(ShipGroupResources.TotalAndAverageLevel, levels.Sum(), levels.Average());
			statusBarText += string.Format(ShipGroupResources.TotalAndAverageExp, exp.Sum(), exp.Average());
		}
		else
		{
			statusBarText += string.Format(ShipGroupResources.ShipCount, group.Members.Count);
			statusBarText += string.Format(ShipGroupResources.TotalAndAverageLevel, levelsum, levelAverage);
			statusBarText += string.Format(ShipGroupResources.TotalAndAverageExp, expsum, expAverage);
		}

		return statusBarText;
	}

	public ShipGroupViewModel() : base("Group", "Group", IconContent.FormShipGroup)
	{
		Db = KCDatabase.Instance;

		FormShipGroup = Ioc.Default.GetService<FormShipGroupTranslationViewModel>()!;

		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += APIUpdated;
		o.ApiGetMember_Ship2.ResponseReceived += APIUpdated;
		o.ApiGetMember_ShipDeck.ResponseReceived += APIUpdated;

		Groups = new(Db.ShipGroup.ShipGroups.Values);
	}

	private void APIUpdated(string apiname, dynamic data)
	{
		SetShips();
	}

	[RelayCommand]
	private void SelectGroup(string name)
	{
		SelectedGroup = Db.ShipGroup.ShipGroups.Values.First(g => g.Name == name);
		SetShips();
	}

	private void SetShips()
	{
		if (SelectedGroup is null) return;

		Ships = new(SelectedGroup.MembersInstance.Where(s => s is not null).Select(s => new ShipGroupItemViewModel(s)));

	}
}
