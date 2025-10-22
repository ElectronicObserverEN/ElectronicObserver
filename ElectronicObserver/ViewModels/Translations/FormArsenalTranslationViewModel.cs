namespace ElectronicObserver.ViewModels.Translations;

public class FormArsenalTranslationViewModel : TranslationBaseViewModel
{
	public string Title => GeneralRes.Arsenal.Replace("_", "__").Replace("&", "_");

	public string MenuMain_ShowShipName => Menus.ShowShipName.Replace("_", "__").Replace("&", "_");
}
