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

	public string ShipView_Fuel => GeneralRes.Fuel;
	public string ShipView_Ammo => GeneralRes.Ammo;
	public string ShipView_Slot1 => ShipGroupResources.ShipView_Slot1;
	public string ShipView_Slot2 => ShipGroupResources.ShipView_Slot2;
	public string ShipView_Slot3 => ShipGroupResources.ShipView_Slot3;
	public string ShipView_Slot4 => ShipGroupResources.ShipView_Slot4;
	public string ShipView_Slot5 => ShipGroupResources.ShipView_Slot5;
	public string ShipView_ExpansionSlot => GeneralRes.Expansion;

	public string ShipView_Aircraft1 => GeneralRes.Planes + " 1";
	public string ShipView_Aircraft2 => GeneralRes.Planes + " 2";
	public string ShipView_Aircraft3 => GeneralRes.Planes + " 3";
	public string ShipView_Aircraft4 => GeneralRes.Planes + " 4";
	public string ShipView_Aircraft5 => GeneralRes.Planes + " 5";
	public string ShipView_AircraftTotal => GeneralRes.Planes + GeneralRes.Total;

	public string ShipView_Fleet => ShipGroupResources.ShipView_Fleet;
	public string ShipView_RepairTime => ShipGroupResources.ShipView_RepairTime;
	public string ShipView_RepairSteel => ShipGroupResources.ShipView_RepairSteel;
	public string ShipView_RepairFuel => ShipGroupResources.ShipView_RepairFuel;

	public string ShipView_Firepower => GeneralRes.Firepower;
	public string ShipView_FirepowerRemain => GeneralRes.Firepower + GeneralRes.ModRemaining;
	public string ShipView_FirepowerTotal => GeneralRes.Firepower + GeneralRes.Total;

	public string ShipView_Torpedo => GeneralRes.Torpedo;
	public string ShipView_TorpedoRemain => GeneralRes.Torpedo + GeneralRes.ModRemaining;
	public string ShipView_TorpedoTotal => GeneralRes.Torpedo + GeneralRes.Total;

	public string ShipView_AA => GeneralRes.AntiAir;
	public string ShipView_AARemain => GeneralRes.AntiAir + GeneralRes.ModRemaining;
	public string ShipView_AATotal => GeneralRes.AntiAir + GeneralRes.Total;

	public string ShipView_Armor => GeneralRes.Armor;
	public string ShipView_ArmorRemain => GeneralRes.Armor + GeneralRes.ModRemaining;
	public string ShipView_ArmorTotal => GeneralRes.Armor + GeneralRes.Total;

	public string ShipView_ASW => GeneralRes.ASW;
	public string ShipView_ASWTotal => GeneralRes.ASW + GeneralRes.Total;

	public string ShipView_Evasion => GeneralRes.Evasion;
	public string ShipView_EvasionTotal => GeneralRes.Evasion + GeneralRes.Total;

	public string ShipView_LOS => GeneralRes.LoS;
	public string ShipView_LOSTotal => GeneralRes.LoS + GeneralRes.Total;

	public string ShipView_Luck => GeneralRes.Luck;
	public string ShipView_LuckRemain => GeneralRes.Luck + GeneralRes.ModRemaining;
	public string ShipView_LuckTotal => GeneralRes.Luck + GeneralRes.Total;

	public string ShipView_BomberTotal => GeneralRes.Bombers + GeneralRes.Total;
	public string ShipView_Speed => GeneralRes.Speed;
	public string ShipView_Range => GeneralRes.Range;

	public string ShipView_AirBattlePower => GeneralRes.Air + GeneralRes.Power;
	public string ShipView_ShellingPower => GeneralRes.Shelling + GeneralRes.Power;
	public string ShipView_AircraftPower => GeneralRes.Bombing + GeneralRes.Power;
	public string ShipView_AntiSubmarinePower => GeneralRes.ASW + GeneralRes.Power;
	public string ShipView_TorpedoPower => GeneralRes.Torpedo + GeneralRes.Power;
	public string ShipView_NightBattlePower => ShipGroupResources.ShipView_NightBattlePower;

	public string ShipView_Locked => GeneralRes.Lock;
	public string ShipView_SallyArea => ShipGroupResources.ShipView_SallyArea;

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
}
