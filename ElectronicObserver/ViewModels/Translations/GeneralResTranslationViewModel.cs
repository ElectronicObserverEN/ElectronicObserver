namespace ElectronicObserver.ViewModels.Translations;

public class GeneralResTranslationViewModel : TranslationBaseViewModel
{
	public string QuestView_Type => GeneralRes.Type.Replace("_", "__").Replace("&", "_");
	public string QuestView_Category => GeneralRes.Category.Replace("_", "__").Replace("&", "_");
	public string QuestView_Name => GeneralRes.QuestName.Replace("_", "__").Replace("&", "_");
	public string QuestView_Progress => GeneralRes.Progress.Replace("_", "__").Replace("&", "_");

	public string MenuProgress_Increment => GeneralRes.IncrementByOne.Replace("_", "__").Replace("&", "_");
	public string MenuProgress_Decrement => GeneralRes.DecrementByOne.Replace("_", "__").Replace("&", "_");

	public string MenuMain_ColumnFilter => GeneralRes.FilterBy.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ColumnFilter_State => GeneralRes.InProgressFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ColumnFilter_Type => GeneralRes.TypeFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ColumnFilter_Category => GeneralRes.CategoryFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ColumnFilter_Name => GeneralRes.NameFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ColumnFilter_Progress => GeneralRes.ProgressFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMain_Initialize => GeneralRes.Initialize.Replace("_", "__").Replace("&", "_");

	public string MenuMain_ShowWeekly => GeneralRes.ShowWeekly.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ShowDaily => GeneralRes.ShowDaily.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ShowOnce => GeneralRes.ShowOneTime.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ShowRunningOnly => GeneralRes.ShowInProgressOnly.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ShowMonthly => GeneralRes.ShowMonthly.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ShowOther => GeneralRes.ShowOther.Replace("_", "__").Replace("&", "_");

	public string Quest => GeneralRes.Quest.Replace("_", "__").Replace("&", "_");

}
