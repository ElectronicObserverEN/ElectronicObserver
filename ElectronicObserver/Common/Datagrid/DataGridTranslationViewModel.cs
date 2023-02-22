using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Common.Datagrid;
public class DataGridTranslationViewModel : TranslationBaseViewModel
{
	public string HideColumn => DataGrid.HideColumn;
	public string OpenColumnSelector => DataGrid.OpenColumnSelector;
	public string ClearSorting => DataGrid.ClearSorting;
}
