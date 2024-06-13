using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window;

namespace ElectronicObserver.Common.Datagrid;
public class ColumnSelectorTranslationViewModel : TranslationBaseViewModel
{
	public string Column => ColumnSelector.Column;
	public string Visible => ColumnSelector.Visible;
	public string Ok => "OK";
	public string Cancel => GeneralRes.Cancel;
	public string ColumnsSelection => ColumnSelector.ColumnsSelection;
	public string ColumnFreezeNumber => ShipGroupColumnFilterResources.ColumnFreezeNumber;
}
