using ElectronicObserver.Window.Dialog;
using DialogAlbumMasterEquipment = ElectronicObserver.Properties.Window.Dialog.DialogAlbumMasterEquipment;

namespace ElectronicObserver.ViewModels.Translations;

public class DialogAlbumMasterEquipmentTranslationViewModel : TranslationBaseViewModel
{
	public string StripMenu_File => DialogAlbumMasterEquipment.StripMenu_File.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_OutputCSVUser => DialogAlbumMasterEquipment.StripMenu_File_OutputCSVUser.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_OutputCSVData => DialogAlbumMasterEquipment.StripMenu_File_OutputCSVData.Replace("_", "__").Replace("&", "_");

	public string StripMenu_Edit => DialogAlbumMasterEquipment.StripMenu_Edit.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_CopyEquipmentName => DialogAlbumMasterEquipment.StripMenu_Edit_CopyEquipmentName.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_CopyEquipmentData => DialogAlbumMasterEquipment.StripMenu_Edit_CopyEquipmentData.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Edit_GoogleEquipmentName => DialogAlbumMasterEquipment.StripMenu_Edit_GoogleEquipmentName.Replace("_", "__").Replace("&", "_");

	public string StripMenu_View => DialogAlbumMasterEquipment.StripMenu_View.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_ShowAppearingArea => DialogAlbumMasterEquipment.StripMenu_View_ShowAppearingArea.Replace("_", "__").Replace("&", "_");

	public string Performance => EncycloRes.Performance;
	public string EquipmentView_Type => DialogAlbumMasterEquipment.EquipmentView_Type.Replace("_", "__").Replace("&", "_");
	public string EquipmentView_Name => DialogAlbumMasterEquipment.EquipmentView_Name.Replace("_", "__").Replace("&", "_");

	public string TitleAircraftCost => DialogAlbumMasterEquipment.TitleAircraftCost.Replace("_", "__").Replace("&", "_");
	public string TitleAircraftDistance => DialogAlbumMasterEquipment.TitleAircraftDistance.Replace("_", "__").Replace("&", "_");
	public string InitialEquipmentShip => DialogAlbumMasterEquipment.InitialEquipmentShip.Replace("_", "__").Replace("&", "_");
	public string LibraryId => DialogAlbumMasterEquipment.LibraryId.Replace("_", "__").Replace("&", "_");
	public string Description => DialogAlbumMasterEquipment.Description.Replace("_", "__").Replace("&", "_");
	public string EquipmentType => DialogAlbumMasterEquipment.EquipmentType.Replace("_", "__").Replace("&", "_");
	public string EquipmentName => DialogAlbumMasterEquipment.EquipmentName.Replace("_", "__").Replace("&", "_");

	public string TitleRange => DialogAlbumMasterEquipment.TitleRange.Replace("_", "__").Replace("&", "_");
	public string TitleSpeed => DialogAlbumMasterEquipment.TitleSpeed.Replace("_", "__").Replace("&", "_");
	public string TitleRarity => EncycloRes.Rarity;
	public string TitleBomber => DialogAlbumMasterEquipment.TitleBomber.Replace("_", "__").Replace("&", "_");
	public string TitleLOS => DialogAlbumMasterEquipment.TitleLOS.Replace("_", "__").Replace("&", "_");
	public string TitleFirepower => DialogAlbumMasterEquipment.TitleFirepower.Replace("_", "__").Replace("&", "_");
	public string TitleTorpedo => DialogAlbumMasterEquipment.TitleTorpedo.Replace("_", "__").Replace("&", "_");
	public string TitleAA => DialogAlbumMasterEquipment.TitleAA.Replace("_", "__").Replace("&", "_");
	public string TitleArmor => DialogAlbumMasterEquipment.TitleArmor.Replace("_", "__").Replace("&", "_");
	public string TitleASW => DialogAlbumMasterEquipment.TitleASW.Replace("_", "__").Replace("&", "_");
	public string TitleEvasion => DialogAlbumMasterEquipment.TitleEvasion.Replace("_", "__").Replace("&", "_");
	public string TitleDismantling => EncycloRes.Dismantling;
	public string TitleUpgradeCost => DialogAlbumMasterEquipment.UpgradeCost;
	public string Interception => DialogAlbumMasterEquipment.Interception.Replace("_", "__").Replace("&", "_");
	public string TitleAccuracy => DialogAlbumMasterEquipment.TitleAccuracy.Replace("_", "__").Replace("&", "_");
	public string AntiBomber => DialogAlbumMasterEquipment.AntiBomber.Replace("_", "__").Replace("&", "_");

	public string SaveCSVDialog => DialogAlbumMasterEquipment.SaveCSVDialog.Replace("_", "__").Replace("&", "_");

	public string Title => DialogAlbumMasterEquipment.Title.Replace("_", "__").Replace("&", "_");
}
