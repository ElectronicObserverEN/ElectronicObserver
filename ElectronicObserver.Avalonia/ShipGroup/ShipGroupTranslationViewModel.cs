using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ShipGroup;

public class ShipGroupTranslationViewModel : ObservableObject
{
	public new void OnPropertyChanged(string? propertyName = null) 
		=> base.OnPropertyChanged(propertyName);

	public string Title => ShipGroupResources.Title;

	public string ShipView_ShipType => ShipGroupResources.ShipView_ShipType;
	public string ShipView_Name => ShipGroupResources.ShipView_Name;
	public string ShipView_NextRemodel => ShipGroupResources.ShipView_NextRemodel;

	public string ShipView_Fuel => ShipGroupResources.Fuel;
	public string ShipView_Ammo => ShipGroupResources.Ammo;
	public string ShipView_Slot1 => ShipGroupResources.ShipView_Slot1;
	public string ShipView_Slot2 => ShipGroupResources.ShipView_Slot2;
	public string ShipView_Slot3 => ShipGroupResources.ShipView_Slot3;
	public string ShipView_Slot4 => ShipGroupResources.ShipView_Slot4;
	public string ShipView_Slot5 => ShipGroupResources.ShipView_Slot5;
	public string ShipView_ExpansionSlot => ShipGroupResources.Expansion;

	public string ShipView_Aircraft1 => ShipGroupResources.Planes + " 1";
	public string ShipView_Aircraft2 => ShipGroupResources.Planes + " 2";
	public string ShipView_Aircraft3 => ShipGroupResources.Planes + " 3";
	public string ShipView_Aircraft4 => ShipGroupResources.Planes + " 4";
	public string ShipView_Aircraft5 => ShipGroupResources.Planes + " 5";
	public string ShipView_AircraftTotal => ShipGroupResources.Planes + ShipGroupResources.Total;

	public string ShipView_Fleet => ShipGroupResources.ShipView_Fleet;
	public string ShipView_RepairTime => ShipGroupResources.ShipView_RepairTime;
	public string ShipView_RepairSteel => ShipGroupResources.ShipView_RepairSteel;
	public string ShipView_RepairFuel => ShipGroupResources.ShipView_RepairFuel;

	public string ShipView_Firepower => ShipGroupResources.Firepower;
	public string ShipView_FirepowerRemain => ShipGroupResources.Firepower + ShipGroupResources.ModRemaining;
	public string ShipView_FirepowerTotal => ShipGroupResources.Firepower + ShipGroupResources.Total;

	public string ShipView_Torpedo => ShipGroupResources.Torpedo;
	public string ShipView_TorpedoRemain => ShipGroupResources.Torpedo + ShipGroupResources.ModRemaining;
	public string ShipView_TorpedoTotal => ShipGroupResources.Torpedo + ShipGroupResources.Total;

	public string ShipView_AA => ShipGroupResources.AntiAir;
	public string ShipView_AARemain => ShipGroupResources.AntiAir + ShipGroupResources.ModRemaining;
	public string ShipView_AATotal => ShipGroupResources.AntiAir + ShipGroupResources.Total;

	public string ShipView_Armor => ShipGroupResources.Armor;
	public string ShipView_ArmorRemain => ShipGroupResources.Armor + ShipGroupResources.ModRemaining;
	public string ShipView_ArmorTotal => ShipGroupResources.Armor + ShipGroupResources.Total;

	public string ShipView_ASW => ShipGroupResources.ASW;
	public string ShipView_ASWTotal => ShipGroupResources.ASW + ShipGroupResources.Total;

	public string ShipView_Evasion => ShipGroupResources.Evasion;
	public string ShipView_EvasionTotal => ShipGroupResources.Evasion + ShipGroupResources.Total;

	public string ShipView_LOS => ShipGroupResources.LoS;
	public string ShipView_LOSTotal => ShipGroupResources.LoS + ShipGroupResources.Total;

	public string ShipView_Luck => ShipGroupResources.Luck;
	public string ShipView_LuckRemain => ShipGroupResources.Luck + ShipGroupResources.ModRemaining;
	public string ShipView_LuckTotal => ShipGroupResources.Luck + ShipGroupResources.Total;

	public string ShipView_BomberTotal => ShipGroupResources.Bombers + ShipGroupResources.Total;
	public string ShipView_Speed => ShipGroupResources.Speed;
	public string ShipView_Range => ShipGroupResources.Range;

	public string ShipView_AirBattlePower => ShipGroupResources.Air + ShipGroupResources.Power;
	public string ShipView_ShellingPower => ShipGroupResources.Shelling + ShipGroupResources.Power;
	public string ShipView_AircraftPower => ShipGroupResources.Bombing + ShipGroupResources.Power;
	public string ShipView_AntiSubmarinePower => ShipGroupResources.ASW + ShipGroupResources.Power;
	public string ShipView_TorpedoPower => ShipGroupResources.Torpedo + ShipGroupResources.Power;
	public string ShipView_NightBattlePower => ShipGroupResources.ShipView_NightBattlePower;

	public string ShipView_Locked => ShipGroupResources.Lock;
	public string ShipView_SallyArea => ShipGroupResources.ShipView_SallyArea;

	public string SortId => ShipGroupResources.SortId;
	public string RepairTimeUnit => ShipGroupResources.RepairTimeUnit;

	public string MenuMember_AddToGroup => ShipGroupResources.MenuMember_AddToGroup;
	public string MenuMember_CreateGroup => ShipGroupResources.MenuMember_CreateGroup;
	public string MenuMember_Exclude => ShipGroupResources.MenuMember_Exclude;
	public string MenuMember_Filter => ShipGroupResources.MenuMember_Filter;
	public string MenuMember_ColumnFilter => ShipGroupResources.MenuMember_ColumnFilter;
	public string MenuMember_SortOrder => ShipGroupResources.MenuMember_SortOrder;
	public string MenuMember_CSVOutput => ShipGroupResources.MenuMember_CSVOutput;

	public string MenuGroup_Add => ShipGroupResources.MenuGroup_Add;
	public string MenuGroup_Copy => ShipGroupResources.MenuGroup_Copy;
	public string MenuGroup_Rename => ShipGroupResources.MenuGroup_Rename;
	public string MenuGroup_Delete => ShipGroupResources.MenuGroup_Delete;
	public string MenuGroup_AutoUpdate => ShipGroupResources.MenuGroup_AutoUpdate;
	public string MenuGroup_ShowStatusBar => ShipGroupResources.MenuGroup_ShowStatusBar;

	public string DialogGroupAddTitle => ShipGroupResources.DialogGroupAddTitle;
	public string DialogGroupAddDescription => ShipGroupResources.DialogGroupAddDescription;
	public string DialogGroupCopyTitle => ShipGroupResources.DialogGroupCopyTitle;
	public string DialogGroupCopyDescription => ShipGroupResources.DialogGroupCopyDescription;
	public string DialogGroupDeleteTitle => ShipGroupResources.DialogGroupDeleteTitle;
	public string DialogGroupDeleteDescription => ShipGroupResources.DialogGroupDeleteDescription;
	public string DialogGroupRenameTitle => ShipGroupResources.DialogGroupRenameTitle;
	public string DialogGroupRenameDescription => ShipGroupResources.DialogGroupRenameDescription;

	public string DataGridSettings => ShipGroupResources.DataGridSettings;
}
