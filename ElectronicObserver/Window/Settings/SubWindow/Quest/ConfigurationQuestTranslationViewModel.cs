using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.SubWindow.Quest;

public class ConfigurationQuestTranslationViewModel : TranslationBaseViewModel
{
	public string FormQuest_ShowRunningOnly => ConfigRes.UnderWayOnly;
	public string ProgressAutoSaving => ConfigRes.ProgressAutoSaving;

	public string Filter => ConfigRes.Filter;
	public string FormQuest_ShowOnce => ConfigRes.OneTimeOther;
	public string FormQuest_ShowDaily => ConfigRes.Daily;
	public string FormQuest_ShowWeekly => ConfigRes.Weekly;
	public string FormQuest_ShowMonthly => ConfigRes.Monthly;
	public string FormQuest_ShowOther => ConfigRes.Others;

	public string FormQuest_AllowUserToSortRows => ConfigRes.AllowUserToSortRows;
}
