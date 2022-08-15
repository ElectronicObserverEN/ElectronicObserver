using ElectronicObserver.Window.Dialog;
using DialogAlbumMasterEquipment = ElectronicObserver.Properties.Window.Dialog.DialogAlbumMasterEquipment;
using DialogAlbumMasterShip = ElectronicObserver.Properties.Window.Dialog.DialogAlbumMasterShip;

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

	public string ShipView_ShipType => EncycloRes.ShipType.Replace("_", "__").Replace("&", "_");
	public string ShipView_Name => EncycloRes.ShipName.Replace("_", "__").Replace("&", "_");

	public string TitleDayAttack => EncycloRes.DayAttack.Replace("_", "__").Replace("&", "_");
	public string TitleNightAttack => EncycloRes.NightAttack.Replace("_", "__").Replace("&", "_");
	public string TitleAirSuperiority => EncycloRes.AirPower.Replace("_", "__").Replace("&", "_");

	// todo: translate
	public string ShipId => "艦船ID:".Replace("_", "__").Replace("&", "_");
	public string LibraryId => DialogAlbumMasterEquipment.LibraryId.Replace("_", "__").Replace("&", "_");

	public string TitleHP => DialogAlbumMasterShip.TitleHP.Replace("_", "__").Replace("&", "_");
	public string Firepower => DialogAlbumMasterShip.Firepower.Replace("_", "__").Replace("&", "_");
	public string Torpedo => DialogAlbumMasterShip.Torpedo.Replace("_", "__").Replace("&", "_");
	public string AA => DialogAlbumMasterShip.AA.Replace("_", "__").Replace("&", "_");
	public string Armor => DialogAlbumMasterShip.Armor.Replace("_", "__").Replace("&", "_");
	public string ASW => DialogAlbumMasterShip.ASW.Replace("_", "__").Replace("&", "_");
	public string Evasion => DialogAlbumMasterShip.Evasion.Replace("_", "__").Replace("&", "_");
	public string Interception => DialogAlbumMasterShip.Interception.Replace("_", "__").Replace("&", "_");
	public string LOS => DialogAlbumMasterShip.LOS.Replace("_", "__").Replace("&", "_");
	public string AntiBomb => DialogAlbumMasterShip.AntiBomb.Replace("_", "__").Replace("&", "_");
	// todo: translation
	public string Luck => EncycloRes.Luck.Replace("_", "__").Replace("&", "_");
	public string Bombing => DialogAlbumMasterShip.Bombing.Replace("_", "__").Replace("&", "_");
	public string Accuracy => DialogAlbumMasterShip.Accuracy.Replace("_", "__").Replace("&", "_");

	public string TitleParameterMax => EncycloRes.Maximum.Replace("_", "__").Replace("&", "_");
	public string TitleParameterMin => EncycloRes.Initial.Replace("_", "__").Replace("&", "_");
	public string BaseValue => EncycloRes.BaseValue.Replace("_", "__").Replace("&", "_");
	public string WithEquipValue => EncycloRes.WithEquipValue.Replace("_", "__").Replace("&", "_");

	public string ShipClassId => DialogAlbumMasterShip.ShipClassId.Replace("_", "__").Replace("&", "_");
	public string ShipClassUnknown => DialogAlbumMasterShip.ShipClassUnknown.Replace("_", "__").Replace("&", "_");
	public string Installation => DialogAlbumMasterShip.Installation.Replace("_", "__").Replace("&", "_");
	public string Equippable => DialogAlbumMasterEquipment.Equippable.Replace("_", "__").Replace("&", "_");

	public string DefaultRange => DialogAlbumMasterShip.DefaultRange.Replace("_", "__").Replace("&", "_");
	public string Empty => DialogAlbumMasterShip.Empty.Replace("_", "__").Replace("&", "_");
	public string ReinforcementSlot => DialogAlbumMasterShip.ReinforcementSlot.Replace("_", "__").Replace("&", "_");

	public string ParameterLevelToolTip => DialogAlbumMasterShip.ParameterLevelToolTip.Replace("_", "__").Replace("&", "_");
	public string ShipBannerToolTip => DialogAlbumMasterShip.ShipBannerToolTip.Replace("_", "__").Replace("&", "_");
	public string ResourceNameToolTip => DialogAlbumMasterShip.ResourceNameToolTip.Replace("_", "__").Replace("&", "_");
	public string HpMinToolTip => DialogAlbumMasterShip.HpMinToolTip.Replace("_", "__").Replace("&", "_");
	public string HpMaxToolTip => DialogAlbumMasterShip.HpMaxToolTip.Replace("_", "__").Replace("&", "_");
	public string RepairTooltip => EncycloRes.RepairTooltip.Replace("_", "__").Replace("&", "_");

	public string RightClickToCopy => DialogAlbumMasterEquipment.RightClickToCopy.Replace("_", "__").Replace("&", "_");
	public string RightClickToOpenInNewWindow => DialogAlbumMasterShip.RightClickToOpenInNewWindow.Replace("_", "__").Replace("&", "_");
	public string RemodelBeforeShipNameToolTip => DialogAlbumMasterShip.RemodelBeforeShipNameToolTip.Replace("_", "__").Replace("&", "_");
	public string ActionReport => DialogAlbumMasterShip.ActionReport.Replace("_", "__").Replace("&", "_");
	public string AviationMaterial => DialogAlbumMasterShip.AviationMaterial.Replace("_", "__").Replace("&", "_");
	public string CsvExportFailed => DialogAlbumMasterShip.CsvExportFailed.Replace("_", "__").Replace("&", "_");
	public string SelectAShip => DialogAlbumMasterShip.SelectAShip.Replace("_", "__").Replace("&", "_");
	public string Unknown => DialogAlbumMasterShip.Unknown.Replace("_", "__").Replace("&", "_");
	public string Recipe => DialogAlbumMasterShip.Recipe.Replace("_", "__").Replace("&", "_");
	public string FailedToFindMapOrRecipe => DialogAlbumMasterShip.FailedToFindMapOrRecipe.Replace("_", "__").Replace("&", "_");
	public string MapOrRecipeSearchCaption => DialogAlbumMasterShip.MapOrRecipeSearchCaption.Replace("_", "__").Replace("&", "_");
	public string SpecifyTargetShip => DialogAlbumMasterShip.SpecifyTargetShip.Replace("_", "__").Replace("&", "_");
	public string NoShipSelectedCaption => DialogAlbumMasterShip.NoShipSelectedCaption.Replace("_", "__").Replace("&", "_");

	public string Title => EncycloRes.ShipEncyclopedia.Replace("_", "__").Replace("&", "_");
}
