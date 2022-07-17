using ElectronicObserver.Window;

namespace ElectronicObserver.ViewModels.Translations;

public class FormQuestTranslationViewModel : TranslationBaseViewModel
{
	public string Title => GeneralRes.Quest.Replace("_", "__").Replace("&", "_");

	public string QuestView_Type => GeneralRes.Type.Replace("_", "__").Replace("&", "_");
	public string QuestView_Category => GeneralRes.Category.Replace("_", "__").Replace("&", "_");
	public string QuestView_Name => GeneralRes.QuestName.Replace("_", "__").Replace("&", "_");
	public string QuestView_Progress => GeneralRes.Progress.Replace("_", "__").Replace("&", "_");

	public string MenuMain_QuestFilter => Properties.Window.FormQuest.MenuMain_QuestFilter.Replace("_", "__").Replace("&", "_");
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

	public string LookUpQuestOnDuckDuckGo => Properties.Window.FormQuest.LookUpQuestOnDuckDuckGo.Replace("_", "__").Replace("&", "_");
	public string LookUpQuestOnStartpage => Properties.Window.FormQuest.LookUpQuestOnStartpage;
	public string LookUpSpecificQuestOnDuckDuckGo => Properties.Window.FormQuest.LookUpSpecificQuestOnDuckDuckGo.Replace("_", "__").Replace("&", "_");
	public string LookUpSpecificQuestOnStartpage => Properties.Window.FormQuest.LookUpSpecificQuestOnStartpage;
	public string MenuMain_KcwikiQuest => Properties.Window.FormQuest.MenuMain_KcwikiQuest.Replace("_", "__").Replace("&", "_");
	public string LookUpSpecificQuestOnWiki => Properties.Window.FormQuest.LookUpSpecificQuestOnWiki.Replace("_", "__").Replace("&", "_");
	public string ShowQuestCode => Properties.Window.FormQuest.ShowQuestCode;
	public string SizeAdjustment => Properties.Window.FormQuest.SizeAdjustment;
	public string Header => Properties.Window.FormQuest.Header;
	public string Row => Properties.Window.FormQuest.Row;
	public string ManuMain_QuestTitle => Properties.Window.FormQuest.ManuMain_QuestTitle.Replace("_", "__").Replace("&", "_");
	public string ManuMain_QuestDescription => Properties.Window.FormQuest.ManuMain_QuestDescription.Replace("_", "__").Replace("&", "_");
	public string ManuMain_QuestTranslate => Properties.Window.FormQuest.ManuMain_QuestTranslate.Replace("_", "__").Replace("&", "_");
	public string MenuMain_Initialize => GeneralRes.Initialize.Replace("_", "__").Replace("&", "_");
	
	
	public string MenuProgress_Increment => GeneralRes.IncrementByOne.Replace("_", "__").Replace("&", "_");
	public string MenuProgress_Decrement => GeneralRes.DecrementByOne.Replace("_", "__").Replace("&", "_");
	public string MenuProgress_Reset => Properties.Window.FormQuest.MenuProgress_Reset.Replace("_", "__").Replace("&", "_");
	public string ModifyProgress => Properties.Window.FormQuest.ModifyProgress;
}
