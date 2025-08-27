using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.DataSubmission;

public class ConfigurationDataSubmissionTranslationViewModel : TranslationBaseViewModel
{
	public string BonodereIntegration => ConfigurationResources.DataSubmission_BonodereIntegration;
	public string DataSubmissionFAQ => ConfigurationResources.DataSubmission_DataSubmissionFAQ;
	public string LoggedInAs => ConfigurationResources.DataSubmission_LoggedInAs;
	public string Login => ConfigurationResources.DataSubmission_Login;
	public string Logout => ConfigurationResources.DataSubmission_Logout;
	public string Password => ConfigurationResources.DataSubmission_Password;
	public string Username => ConfigurationResources.DataSubmission_Username;

	public string EnableTsunDbSubmission => ConfigurationResources.Control_EnableTsunDbSubmission;
	public string PrivacyPolicy => ConfigurationResources.DataSubmission_PrivacyPolicy;

	public string Details => PoiDbSubmissionResources.Details;
}
