using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.AutoRefresh;

public class AutoRefreshTranslationViewModel : TranslationBaseViewModel
{
	public string Title => AutoRefresh.Title;

	public string SingleMapMode => AutoRefresh.SingleMapMode;
	public string SingleMapModeToolTip => AutoRefresh.SingleMapModeToolTip;
	public string SingleMapModeIsEnabled => AutoRefresh.SingleMapModeIsEnabled;

	public string Enabled => AutoRefresh.Enabled;
	public string RemoveRule => AutoRefresh.RemoveRule;

	public string AllowedCells => AutoRefresh.AllowedCells;

	public string RemoveCell => AutoRefresh.RemoveCell;

}
