using ElectronicObserver.Properties.Data;
using ElectronicObserver.Properties.Window;
using ElectronicObserver.Properties.Window.Dialog;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public class SortieRecordViewerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => SortieRecordViewer.Title;

	public string File => FormMain.File;
	public string CopySortieData => SortieRecordViewer.CopySortieData;
	public string LoadSortieData => SortieRecordViewer.LoadSortieData;
	public string AirControlSimulator => Tools.AirControlSimulator.AirControlSimulator.Title;
	public string CopyLink => SortieRecordViewer.CopyLink;
	public string Open => SortieRecordViewer.Open;

	public string Start => DialogDropRecordViewer.Start;
	public string End => DialogDropRecordViewer.End;

	public string Search => SortieRecordViewer.Search;
	public string Cancel => GeneralRes.Cancel;

	public string World => SortieRecordViewer.World;
	public string Map => SortieRecordViewer.Map;

	public string FleetImage => SortieRecordViewer.FleetImage;
	public string Replay => SortieRecordViewer.Replay;
	public string CopyData => SortieRecordViewer.CopyData;
	public string SortieDetail => SortieRecordViewer.SortieDetail;
	public string SmokeScreenCsv => $"{BattleRes.SmokeScreen} CSV";

	public string FailedToParseApiData => SortieRecordViewer.FailedToParseApiData;

	public string SelectedItems => SortieRecordViewer.SelectedItems;
}
