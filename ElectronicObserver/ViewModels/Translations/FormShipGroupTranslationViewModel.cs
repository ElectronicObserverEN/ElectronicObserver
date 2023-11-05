namespace ElectronicObserver.ViewModels.Translations;

public class FormShipGroupTranslationViewModel : TranslationBaseViewModel
{
	public string Title => ShipGroupResources.Title.Replace("_", "__").Replace("&", "_");

	public string ShipView_ShipType => ShipGroupResources.ShipView_ShipType.Replace("_", "__").Replace("&", "_");
	public string ShipView_Name => ShipGroupResources.ShipView_Name.Replace("_", "__").Replace("&", "_");
	public string ShipView_NextRemodel => ShipGroupResources.ShipView_NextRemodel.Replace("_", "__").Replace("&", "_");

	public string ShipView_Fuel => GeneralRes.Fuel.Replace("_", "__").Replace("&", "_");
	public string ShipView_Ammo => GeneralRes.Ammo.Replace("_", "__").Replace("&", "_");
	public string ShipView_Slot1 => ShipGroupResources.ShipView_Slot1.Replace("_", "__").Replace("&", "_");
	public string ShipView_Slot2 => ShipGroupResources.ShipView_Slot2.Replace("_", "__").Replace("&", "_");
	public string ShipView_Slot3 => ShipGroupResources.ShipView_Slot3.Replace("_", "__").Replace("&", "_");
	public string ShipView_Slot4 => ShipGroupResources.ShipView_Slot4.Replace("_", "__").Replace("&", "_");
	public string ShipView_Slot5 => ShipGroupResources.ShipView_Slot5.Replace("_", "__").Replace("&", "_");
	public string ShipView_ExpansionSlot => GeneralRes.Expansion.Replace("_", "__").Replace("&", "_");

	public string ShipView_Aircraft1 => GeneralRes.Planes + " 1".Replace("_", "__").Replace("&", "_");
	public string ShipView_Aircraft2 => GeneralRes.Planes + " 2".Replace("_", "__").Replace("&", "_");
	public string ShipView_Aircraft3 => GeneralRes.Planes + " 3".Replace("_", "__").Replace("&", "_");
	public string ShipView_Aircraft4 => GeneralRes.Planes + " 4".Replace("_", "__").Replace("&", "_");
	public string ShipView_Aircraft5 => GeneralRes.Planes + " 5".Replace("_", "__").Replace("&", "_");
	public string ShipView_AircraftTotal => GeneralRes.Planes + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_Fleet => ShipGroupResources.ShipView_Fleet.Replace("_", "__").Replace("&", "_");
	public string ShipView_RepairTime => ShipGroupResources.ShipView_RepairTime.Replace("_", "__").Replace("&", "_");
	public string ShipView_RepairSteel => ShipGroupResources.ShipView_RepairSteel.Replace("_", "__").Replace("&", "_");
	public string ShipView_RepairFuel => ShipGroupResources.ShipView_RepairFuel.Replace("_", "__").Replace("&", "_");

	public string ShipView_Firepower => GeneralRes.Firepower.Replace("_", "__").Replace("&", "_");
	public string ShipView_FirepowerRemain => GeneralRes.Firepower + GeneralRes.ModRemaining.Replace("_", "__").Replace("&", "_");
	public string ShipView_FirepowerTotal => GeneralRes.Firepower + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_Torpedo => GeneralRes.Torpedo.Replace("_", "__").Replace("&", "_");
	public string ShipView_TorpedoRemain => GeneralRes.Torpedo + GeneralRes.ModRemaining.Replace("_", "__").Replace("&", "_");
	public string ShipView_TorpedoTotal => GeneralRes.Torpedo + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_AA => GeneralRes.AntiAir.Replace("_", "__").Replace("&", "_");
	public string ShipView_AARemain => GeneralRes.AntiAir + GeneralRes.ModRemaining.Replace("_", "__").Replace("&", "_");
	public string ShipView_AATotal => GeneralRes.AntiAir + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_Armor => GeneralRes.Armor.Replace("_", "__").Replace("&", "_");
	public string ShipView_ArmorRemain => GeneralRes.Armor + GeneralRes.ModRemaining.Replace("_", "__").Replace("&", "_");
	public string ShipView_ArmorTotal => GeneralRes.Armor + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_ASW => GeneralRes.ASW.Replace("_", "__").Replace("&", "_");
	public string ShipView_ASWTotal => GeneralRes.ASW + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_Evasion => GeneralRes.Evasion.Replace("_", "__").Replace("&", "_");
	public string ShipView_EvasionTotal => GeneralRes.Evasion + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_LOS => GeneralRes.LoS.Replace("_", "__").Replace("&", "_");
	public string ShipView_LOSTotal => GeneralRes.LoS + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_Luck => GeneralRes.Luck.Replace("_", "__").Replace("&", "_");
	public string ShipView_LuckRemain => GeneralRes.Luck + GeneralRes.ModRemaining.Replace("_", "__").Replace("&", "_");
	public string ShipView_LuckTotal => GeneralRes.Luck + GeneralRes.Total.Replace("_", "__").Replace("&", "_");

	public string ShipView_BomberTotal => GeneralRes.Bombers + GeneralRes.Total.Replace("_", "__").Replace("&", "_");
	public string ShipView_Speed => GeneralRes.Speed.Replace("_", "__").Replace("&", "_");
	public string ShipView_Range => GeneralRes.Range.Replace("_", "__").Replace("&", "_");

	public string ShipView_AirBattlePower => GeneralRes.Air + GeneralRes.Power.Replace("_", "__").Replace("&", "_");
	public string ShipView_ShellingPower => GeneralRes.Shelling + GeneralRes.Power.Replace("_", "__").Replace("&", "_");
	public string ShipView_AircraftPower => GeneralRes.Bombing + GeneralRes.Power.Replace("_", "__").Replace("&", "_");
	public string ShipView_AntiSubmarinePower => GeneralRes.ASW + GeneralRes.Power.Replace("_", "__").Replace("&", "_");
	public string ShipView_TorpedoPower => GeneralRes.Torpedo + GeneralRes.Power.Replace("_", "__").Replace("&", "_");
	public string ShipView_NightBattlePower => ShipGroupResources.ShipView_NightBattlePower.Replace("_", "__").Replace("&", "_");

	public string ShipView_Locked => GeneralRes.Lock.Replace("_", "__").Replace("&", "_");
	public string ShipView_SallyArea => ShipGroupResources.ShipView_SallyArea.Replace("_", "__").Replace("&", "_");

	public string MenuMember_AddToGroup => ShipGroupResources.MenuMember_AddToGroup.Replace("_", "__").Replace("&", "_");
	public string MenuMember_CreateGroup => ShipGroupResources.MenuMember_CreateGroup.Replace("_", "__").Replace("&", "_");
	public string MenuMember_Exclude => ShipGroupResources.MenuMember_Exclude.Replace("_", "__").Replace("&", "_");
	public string MenuMember_Filter => ShipGroupResources.MenuMember_Filter.Replace("_", "__").Replace("&", "_");
	public string MenuMember_ColumnFilter => ShipGroupResources.MenuMember_ColumnFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMember_SortOrder => ShipGroupResources.MenuMember_SortOrder.Replace("_", "__").Replace("&", "_");
	public string MenuMember_CSVOutput => ShipGroupResources.MenuMember_CSVOutput.Replace("_", "__").Replace("&", "_");

	public string MenuGroup_Add => ShipGroupResources.MenuGroup_Add.Replace("_", "__").Replace("&", "_");
	public string MenuGroup_Copy => ShipGroupResources.MenuGroup_Copy.Replace("_", "__").Replace("&", "_");
	public string MenuGroup_Rename => ShipGroupResources.MenuGroup_Rename.Replace("_", "__").Replace("&", "_");
	public string MenuGroup_Delete => ShipGroupResources.MenuGroup_Delete.Replace("_", "__").Replace("&", "_");
	public string MenuGroup_AutoUpdate => ShipGroupResources.MenuGroup_AutoUpdate.Replace("_", "__").Replace("&", "_");
	public string MenuGroup_ShowStatusBar => ShipGroupResources.MenuGroup_ShowStatusBar.Replace("_", "__").Replace("&", "_");

	public string DialogGroupAddTitle => ShipGroupResources.DialogGroupAddTitle.Replace("_", "__").Replace("&", "_");
	public string DialogGroupAddDescription => ShipGroupResources.DialogGroupAddDescription.Replace("_", "__").Replace("&", "_");
	public string DialogGroupCopyTitle => ShipGroupResources.DialogGroupCopyTitle.Replace("_", "__").Replace("&", "_");
	public string DialogGroupCopyDescription => ShipGroupResources.DialogGroupCopyDescription.Replace("_", "__").Replace("&", "_");
	public string DialogGroupDeleteTitle => ShipGroupResources.DialogGroupDeleteTitle.Replace("_", "__").Replace("&", "_");
	public string DialogGroupDeleteDescription => ShipGroupResources.DialogGroupDeleteDescription.Replace("_", "__").Replace("&", "_");
	public string DialogGroupRenameTitle => ShipGroupResources.DialogGroupRenameTitle.Replace("_", "__").Replace("&", "_");
	public string DialogGroupRenameDescription => ShipGroupResources.DialogGroupRenameDescription.Replace("_", "__").Replace("&", "_");
}
