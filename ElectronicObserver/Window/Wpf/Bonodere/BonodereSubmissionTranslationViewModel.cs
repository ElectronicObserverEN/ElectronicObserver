using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereSubmissionTranslationViewModel : TranslationBaseViewModel
{
	public string InconsistantDataDetected => BonodereSubmissionResources.InconsistantDataDetected;
	public string Login => BonodereSubmissionResources.Login;
	public string Logout => BonodereSubmissionResources.Logout;
	public string SubmitData => BonodereSubmissionResources.SubmitData;
}
