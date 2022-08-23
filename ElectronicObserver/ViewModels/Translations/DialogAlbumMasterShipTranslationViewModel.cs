using ElectronicObserver.Properties.Window.Dialog;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.ViewModels.Translations;

public class DialogAlbumMasterShipTranslationViewModel : TranslationBaseViewModel
{
	public string StripMenu_File => DialogAlbumMasterEquipment.StripMenu_File.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_OutputCSVUser => DialogAlbumMasterEquipment.StripMenu_File_OutputCSVUser.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_OutputCSVData => DialogAlbumMasterEquipment.StripMenu_File_OutputCSVData.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_MergeDefaultRecord => DialogAlbumMasterShip.StripMenu_File_MergeDefaultRecord.Replace("_", "__").Replace("&", "_");

	public string StripMenu_Edit => DialogAlbumMasterEquipment.StripMenu_Edit.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_EditParameter => DialogAlbumMasterShip.StripMenu_Edit_EditParameter.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_CopyShipName => DialogAlbumMasterShip.StripMenu_Edit_CopyShipName.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_CopyShipData => DialogAlbumMasterShip.StripMenu_Edit_CopyShipData.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_GoogleShipName => DialogAlbumMasterShip.StripMenu_Edit_GoogleShipName.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_CopySpecialEquipmentTable => DialogAlbumMasterShip.StripMenu_Edit_CopySpecialEquipmentTable.Replace("_", "__").Replace("&", "_");

	public string StripMenu_View => DialogAlbumMasterEquipment.StripMenu_View.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_ShowAppearingArea => DialogAlbumMasterEquipment.StripMenu_View_ShowAppearingArea.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_ShowShipGraphicViewer => DialogAlbumMasterShip.StripMenu_View_ShowShipGraphicViewer.Replace("_", "__").Replace("&", "_");

	public string ShipView_ShipType => EncycloRes.ShipType;
	public string ShipView_Name => EncycloRes.ShipName;

	public string TitleDayAttack => EncycloRes.DayAttack;
	public string TitleNightAttack => EncycloRes.NightAttack;
	public string TitleAirSuperiority => EncycloRes.AirPower;

	public string ShipId => DialogAlbumMasterShip.ShipId;
	public string LibraryId => DialogAlbumMasterEquipment.LibraryId;

	public string TitleHP => DialogAlbumMasterShip.TitleHP;
	public string Firepower => DialogAlbumMasterShip.Firepower;
	public string Torpedo => DialogAlbumMasterShip.Torpedo;
	public string AA => DialogAlbumMasterShip.AA;
	public string Armor => DialogAlbumMasterShip.Armor;
	public string ASW => DialogAlbumMasterShip.ASW;
	public string Evasion => DialogAlbumMasterShip.Evasion;
	public string Interception => DialogAlbumMasterShip.Interception;
	public string LOS => DialogAlbumMasterShip.LOS;
	public string AntiBomb => DialogAlbumMasterShip.AntiBomb;
	public string Luck => DialogAlbumMasterShip.Luck;
	public string Bombing => DialogAlbumMasterShip.Bombing;
	public string Accuracy => DialogAlbumMasterShip.Accuracy;

	public string TitleParameterMax => EncycloRes.Maximum;
	public string TitleParameterMin => EncycloRes.Initial;
	public string BaseValue => EncycloRes.BaseValue;
	public string WithEquipValue => EncycloRes.WithEquipValue;

	public string ShipClassId => DialogAlbumMasterShip.ShipClassId;
	public string ShipClassUnknown => DialogAlbumMasterShip.ShipClassUnknown;
	public string Installation => DialogAlbumMasterShip.Installation;
	public string Equippable => DialogAlbumMasterEquipment.Equippable;

	public string DefaultRange => DialogAlbumMasterShip.DefaultRange;
	public string Empty => DialogAlbumMasterShip.Empty;
	public string ReinforcementSlot => DialogAlbumMasterShip.ReinforcementSlot;

	public string ParameterLevelToolTip => DialogAlbumMasterShip.ParameterLevelToolTip;
	public string ShipBannerToolTip => DialogAlbumMasterShip.ShipBannerToolTip;
	public string ResourceNameToolTip => DialogAlbumMasterShip.ResourceNameToolTip;
	public string HpMinToolTip => DialogAlbumMasterShip.HpMinToolTip;
	public string HpMaxToolTip => DialogAlbumMasterShip.HpMaxToolTip;
	public string RepairTooltip => EncycloRes.RepairTooltip;

	public string RightClickToCopy => DialogAlbumMasterEquipment.RightClickToCopy;
	public string RightClickToOpenInNewWindow => DialogAlbumMasterShip.RightClickToOpenInNewWindow;
	public string RemodelBeforeShipNameToolTip => DialogAlbumMasterShip.RemodelBeforeShipNameToolTip;
	public string ActionReport => DialogAlbumMasterShip.ActionReport;
	public string AviationMaterial => DialogAlbumMasterShip.AviationMaterial;
	public string CsvExportFailed => DialogAlbumMasterShip.CsvExportFailed;
	public string SelectAShip => DialogAlbumMasterShip.SelectAShip;
	public string Unknown => DialogAlbumMasterShip.Unknown;
	public string Recipe => DialogAlbumMasterShip.Recipe;
	public string FailedToFindMapOrRecipe => DialogAlbumMasterShip.FailedToFindMapOrRecipe;
	public string MapOrRecipeSearchCaption => DialogAlbumMasterShip.MapOrRecipeSearchCaption;
	public string SpecifyTargetShip => DialogAlbumMasterShip.SpecifyTargetShip;
	public string NoShipSelectedCaption => DialogAlbumMasterShip.NoShipSelectedCaption;

	public string Title => EncycloRes.ShipEncyclopedia;
}
