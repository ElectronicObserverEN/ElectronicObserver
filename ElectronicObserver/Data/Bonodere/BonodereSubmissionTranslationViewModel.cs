using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Data.Bonodere;

public class BonodereSubmissionTranslationViewModel : TranslationBaseViewModel
{
	public string InconsistantDataDetected => BonodereSubmissionResources.InconsistantDataDetected;
	public string Error => BonodereSubmissionResources.BonodereError;
	public string Success => BonodereSubmissionResources.BonodereSubmissionSuccess;
}
