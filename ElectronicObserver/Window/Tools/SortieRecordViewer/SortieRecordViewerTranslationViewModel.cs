using ElectronicObserver.Properties.Window.Dialog;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public class SortieRecordViewerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => SortieRecordViewer.Title;

	public string Start => DialogDropRecordViewer.Start;
	public string End => DialogDropRecordViewer.End;

	public string Search => SortieRecordViewer.Search;

	public string FleetImage => SortieRecordViewer.FleetImage;
	public string CopyReplay => SortieRecordViewer.CopyReplay;

	public string FailedToParseApiData => SortieRecordViewer.FailedToParseApiData;
}
