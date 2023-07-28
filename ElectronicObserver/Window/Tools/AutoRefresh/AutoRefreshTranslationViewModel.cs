using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.AutoRefresh;

public class AutoRefreshTranslationViewModel : TranslationBaseViewModel
{
	public string Title => AutoRefreshResources.Title;

	public string NoInteractionWarning => AutoRefreshResources.NoInteractionWarning;

	public string SingleMapMode => AutoRefreshResources.SingleMapMode;
	public string SingleMapModeToolTip => AutoRefreshResources.SingleMapModeToolTip;
	public string SingleMapModeIsEnabled => AutoRefreshResources.SingleMapModeIsEnabled;

	public string Enabled => AutoRefreshResources.Enabled;
	public string RemoveRule => AutoRefreshResources.RemoveRule;

	public string AllowedCells => AutoRefreshResources.AllowedCells;

	public string RemoveCell => AutoRefreshResources.RemoveCell;

}
