namespace ElectronicObserver.ViewModels.Translations;

public class FormQuestTranslationViewModel : TranslationBaseViewModel
{
	public string Title => GeneralRes.Quest.Replace("_", "__").Replace("&", "_");

	public string QuestView_Type => GeneralRes.Type.Replace("_", "__").Replace("&", "_");
	public string QuestView_Category => GeneralRes.Category.Replace("_", "__").Replace("&", "_");
	public string QuestView_Name => GeneralRes.QuestName.Replace("_", "__").Replace("&", "_");
	public string QuestView_Progress => GeneralRes.Progress.Replace("_", "__").Replace("&", "_");
	public string QuestView_ProgressResetsDaily => GeneralRes.ProgressResetsDaily;
	public string QuestView_EndsOn => GeneralRes.QuestEndsOn;

	public string MenuMain_QuestFilter => QuestResources.MenuMain_QuestFilter.Replace("_", "__").Replace("&", "_");
	public string ShowInProgressOnly => GeneralRes.ShowInProgressOnly.Replace("_", "__").Replace("&", "_");
	public string ShowOneTime => GeneralRes.ShowOneTime.Replace("_", "__").Replace("&", "_");
	public string ShowDaily => GeneralRes.ShowDaily.Replace("_", "__").Replace("&", "_");
	public string ShowWeekly => GeneralRes.ShowWeekly.Replace("_", "__").Replace("&", "_");
	public string ShowMonthly => GeneralRes.ShowMonthly.Replace("_", "__").Replace("&", "_");
	public string ShowOther => GeneralRes.ShowOther.Replace("_", "__").Replace("&", "_");

	public string MenuMain_ColumnFilter => GeneralRes.FilterBy.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ColumnFilter_State => GeneralRes.InProgressFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ColumnFilter_Type => GeneralRes.TypeFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ColumnFilter_Category => GeneralRes.CategoryFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ColumnFilter_Name => GeneralRes.NameFilter.Replace("_", "__").Replace("&", "_");
	public string MenuMain_ColumnFilter_Progress => GeneralRes.ProgressFilter.Replace("_", "__").Replace("&", "_");

	public string LookUpQuestOnDuckDuckGo => QuestResources.LookUpQuestOnDuckDuckGo.Replace("_", "__").Replace("&", "_");
	public string LookUpQuestOnStartpage => QuestResources.LookUpQuestOnStartpage;
	public string LookUpSpecificQuestOnDuckDuckGo => QuestResources.LookUpSpecificQuestOnDuckDuckGo.Replace("_", "__").Replace("&", "_");
	public string LookUpSpecificQuestOnStartpage => QuestResources.LookUpSpecificQuestOnStartpage;
	public string MenuMain_KcwikiQuest => QuestResources.MenuMain_KcwikiQuest.Replace("_", "__").Replace("&", "_");
	public string LookUpSpecificQuestOnWiki => QuestResources.LookUpSpecificQuestOnWiki.Replace("_", "__").Replace("&", "_");
	public string ShowQuestCode => QuestResources.ShowQuestCode;
	public string SizeAdjustment => QuestResources.SizeAdjustment;
	public string Header => QuestResources.Header;
	public string Row => QuestResources.Row;
	public string ManuMain_QuestTitle => QuestResources.ManuMain_QuestTitle.Replace("_", "__").Replace("&", "_");
	public string ManuMain_QuestDescription => QuestResources.ManuMain_QuestDescription.Replace("_", "__").Replace("&", "_");
	public string ManuMain_QuestTranslate => QuestResources.ManuMain_QuestTranslate.Replace("_", "__").Replace("&", "_");
	public string MenuMain_Initialize => GeneralRes.Initialize.Replace("_", "__").Replace("&", "_");
	
	
	public string MenuProgress_Increment => GeneralRes.IncrementByOne.Replace("_", "__").Replace("&", "_");
	public string MenuProgress_Decrement => GeneralRes.DecrementByOne.Replace("_", "__").Replace("&", "_");
	public string MenuProgress_Reset => QuestResources.MenuProgress_Reset.Replace("_", "__").Replace("&", "_");
	public string ModifyProgress => QuestResources.ModifyProgress;
}
