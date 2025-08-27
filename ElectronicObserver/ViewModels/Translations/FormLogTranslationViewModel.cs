namespace ElectronicObserver.ViewModels.Translations;

public class FormLogTranslationViewModel : TranslationBaseViewModel
{
	public string Title => GeneralRes.Log;
	public string Clear => GeneralRes.Clear.Replace("_", "__").Replace("&", "_");
}
