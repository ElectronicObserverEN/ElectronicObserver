namespace ElectronicObserver.ViewModels.Translations;

public class DialogAlbumMasterShipTranslationViewModel : TranslationBaseViewModel
{
	public string StripMenu_File => AlbumMasterEquipmentResources.StripMenu_File.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_OutputCSVUser => AlbumMasterEquipmentResources.StripMenu_File_OutputCSVUser.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_OutputCSVData => AlbumMasterEquipmentResources.StripMenu_File_OutputCSVData.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_MergeDefaultRecord => AlbumMasterShipResources.StripMenu_File_MergeDefaultRecord.Replace("_", "__").Replace("&", "_");

	public string StripMenu_Edit => AlbumMasterEquipmentResources.StripMenu_Edit.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_EditParameter => AlbumMasterShipResources.StripMenu_Edit_EditParameter.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_CopyShipName => AlbumMasterShipResources.StripMenu_Edit_CopyShipName.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_CopyShipData => AlbumMasterShipResources.StripMenu_Edit_CopyShipData.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_GoogleShipName => AlbumMasterShipResources.StripMenu_Edit_GoogleShipName.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_CopySpecialEquipmentTable => AlbumMasterShipResources.StripMenu_Edit_CopySpecialEquipmentTable.Replace("_", "__").Replace("&", "_");

	public string StripMenu_View => AlbumMasterEquipmentResources.StripMenu_View.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_ShowAppearingArea => AlbumMasterEquipmentResources.StripMenu_View_ShowAppearingArea.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_ShowShipGraphicViewer => AlbumMasterShipResources.StripMenu_View_ShowShipGraphicViewer.Replace("_", "__").Replace("&", "_");

	public string ShipView_ShipType => EncycloRes.ShipType;
	public string ShipView_Name => EncycloRes.ShipName;

	public string TitleDayAttack => EncycloRes.DayAttack;
	public string TitleNightAttack => EncycloRes.NightAttack;
	public string TitleAirSuperiority => EncycloRes.AirPower;

	public string ShipId => AlbumMasterShipResources.ShipId;
	public string LibraryId => AlbumMasterEquipmentResources.LibraryId;

	public string TitleHP => AlbumMasterShipResources.TitleHP;
	public string Firepower => AlbumMasterShipResources.Firepower;
	public string Torpedo => AlbumMasterShipResources.Torpedo;
	public string AA => AlbumMasterShipResources.AA;
	public string Armor => AlbumMasterShipResources.Armor;
	public string ASW => AlbumMasterShipResources.ASW;
	public string Evasion => AlbumMasterShipResources.Evasion;
	public string Interception => AlbumMasterShipResources.Interception;
	public string LOS => AlbumMasterShipResources.LOS;
	public string AntiBomb => AlbumMasterShipResources.AntiBomb;
	public string Luck => AlbumMasterShipResources.Luck;
	public string Bombing => AlbumMasterShipResources.Bombing;
	public string Accuracy => AlbumMasterShipResources.Accuracy;
	public string TitleSpeed => EncycloRes.Speed;
	public string TitleRarity => EncycloRes.Rarity;
	public string TitleConsumption => EncycloRes.Consumption;

	public string TitleParameterMax => EncycloRes.Maximum;
	public string TitleParameterMin => EncycloRes.Initial;
	public string BaseValue => EncycloRes.BaseValue;
	public string WithEquipValue => EncycloRes.WithEquipValue;

	public string ShipClassId => AlbumMasterShipResources.ShipClassId;
	public string ShipClassUnknown => AlbumMasterShipResources.ShipClassUnknown;
	public string Installation => AlbumMasterShipResources.Installation;
	public string Equippable => AlbumMasterEquipmentResources.Equippable;

	public string DefaultRange => AlbumMasterShipResources.DefaultRange;
	public string Empty => AlbumMasterShipResources.Empty;
	public string ReinforcementSlot => AlbumMasterShipResources.ReinforcementSlot;

	public string TitleConstructionTime => EncycloRes.ConstructionTime;
	public string TitleDismantling => EncycloRes.Dismantling;
	public string TitleModernization => EncycloRes.Modernization;

	public string TitleBeforeRemodel => EncycloRes.BeforeRemodel;
	public string TitleAfterRemodel => EncycloRes.AfterRemodel;

	public string ParameterLevelToolTip => AlbumMasterShipResources.ParameterLevelToolTip;
	public string ShipBannerToolTip => AlbumMasterShipResources.ShipBannerToolTip;
	public string ResourceNameToolTip => AlbumMasterShipResources.ResourceNameToolTip;
	public string HpMinToolTip => AlbumMasterShipResources.HpMinToolTip;
	public string HpMaxToolTip => AlbumMasterShipResources.HpMaxToolTip;
	public string RepairTooltip => EncycloRes.RepairTooltip;

	public string RightClickToCopy => AlbumMasterEquipmentResources.RightClickToCopy;
	public string RightClickToOpenInNewWindow => AlbumMasterShipResources.RightClickToOpenInNewWindow;
	public string RemodelBeforeShipNameToolTip => AlbumMasterShipResources.RemodelBeforeShipNameToolTip;
	public string ActionReport => AlbumMasterShipResources.ActionReport;
	public string AviationMaterial => AlbumMasterShipResources.AviationMaterial;
	public string CsvExportFailed => AlbumMasterShipResources.CsvExportFailed;
	public string SelectAShip => AlbumMasterShipResources.SelectAShip;
	public string Unknown => AlbumMasterShipResources.Unknown;
	public string Recipe => AlbumMasterShipResources.Recipe;
	public string FailedToFindMapOrRecipe => AlbumMasterShipResources.FailedToFindMapOrRecipe;
	public string MapOrRecipeSearchCaption => AlbumMasterShipResources.MapOrRecipeSearchCaption;
	public string SpecifyTargetShip => AlbumMasterShipResources.SpecifyTargetShip;
	public string NoShipSelectedCaption => AlbumMasterShipResources.NoShipSelectedCaption;

	public string SpecialEquipmentTableHeader1 => AlbumMasterShipResources.SpecialEquipmentTableHeader1;
	public string SpecialEquipmentTableHeader2 => AlbumMasterShipResources.SpecialEquipmentTableHeader2;

	public string Title => EncycloRes.ShipEncyclopedia;
}
