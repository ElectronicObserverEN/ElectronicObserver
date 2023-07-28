using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public class SortieRecordViewerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => SortieRecordViewerResources.Title;

	public string File => MainResources.File;
	public string CopySortieData => SortieRecordViewerResources.CopySortieData;
	public string LoadSortieData => SortieRecordViewerResources.LoadSortieData;
	public string AirControlSimulator => AirControlSimulatorResources.Title;
	public string CopyLink => SortieRecordViewerResources.CopyLink;
	public string Open => SortieRecordViewerResources.Open;

	public string Start => DropRecordViewerResources.Start;
	public string End => DropRecordViewerResources.End;

	public string Search => SortieRecordViewerResources.Search;
	public string Cancel => GeneralRes.Cancel;

	public string World => SortieRecordViewerResources.World;
	public string Map => SortieRecordViewerResources.Map;

	public string FleetImage => SortieRecordViewerResources.FleetImage;
	public string Replay => SortieRecordViewerResources.Replay;
	public string CopyData => SortieRecordViewerResources.CopyData;
	public string SortieDetail => SortieRecordViewerResources.SortieDetail;
	public string SmokeScreenCsv => $"{BattleRes.SmokeScreen} CSV";

	public string FailedToParseApiData => SortieRecordViewerResources.FailedToParseApiData;

	public string SelectedItems => SortieRecordViewerResources.SelectedItems;
}
