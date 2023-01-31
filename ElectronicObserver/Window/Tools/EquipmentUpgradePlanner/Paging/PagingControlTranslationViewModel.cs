using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Paging;

public class PagingControlTranslationViewModel : TranslationBaseViewModel
{
	public string ItemsPerPage => PagingControl.ItemsPerPage;
	public string Page => PagingControl.Page;
}
