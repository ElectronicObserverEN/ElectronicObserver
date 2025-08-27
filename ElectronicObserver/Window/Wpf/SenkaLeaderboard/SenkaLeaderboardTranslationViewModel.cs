using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Wpf.SenkaLeaderboard;

public class SenkaLeaderboardTranslationViewModel : TranslationBaseViewModel
{
	public string LoadedEntries => SenkaLeaderboardResources.LoadedEntries;
	public string SubmitData => SenkaLeaderboardResources.SubmitData;
	public string Title => SenkaLeaderboardResources.Title;

	public string Rank => SenkaLeaderboardResources.Rank;
	public string Admiral => SenkaLeaderboardResources.Admiral;
	public string Senka => SenkaViewerResources.Senka;
	public string Medal => HeadquartersResources.ItemNameFirstClassMedal;
	public string AdmiralComment => SenkaLeaderboardResources.AdmiralComment;
}
