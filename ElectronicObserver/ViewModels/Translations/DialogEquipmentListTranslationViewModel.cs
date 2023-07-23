namespace ElectronicObserver.ViewModels.Translations;

public class DialogEquipmentListTranslationViewModel : TranslationBaseViewModel
{
	public string Title => EncycloRes.EquipmentList;

	public string TopMenu_File => EquipmentListResources.TopMenu_File.Replace("_", "__").Replace("&", "_");
	public string TopMenu_File_CSVOutput => EquipmentListResources.TopMenu_File_CSVOutput.Replace("_", "__").Replace("&", "_");
	public string TopMenu_File_Update => EquipmentListResources.TopMenu_File_Update.Replace("_", "__").Replace("&", "_");
	public string TopMenu_File_CopyToFleetAnalysis => EquipmentListResources.TopMenu_File_CopyToFleetAnalysis.Replace("_", "__").Replace("&", "_");

	public string View => EquipmentListResources.View;
	public string ShowLockedEquipmentOnly => EquipmentListResources.ShowLockedEquipmentOnly;

	public string EquipmentView_Name => EncycloRes.EquipName;
	public string EquipmentView_CountAll => EncycloRes.CountAll;
	public string EquipmentView_CountRemain => EncycloRes.CountExtra;

	public string DetailView_Level => EncycloRes.StarLevel;
	public string DetailView_AircraftLevel => EncycloRes.SkillLevel;
	public string DetailView_CountAll => EncycloRes.CountAll;
	public string DetailView_CountRemain => EncycloRes.CountExtra;

	public string SaveCSVDialog => EquipmentListResources.SaveCSVDialog;
	public string Error => EquipmentListResources.Error;
}
