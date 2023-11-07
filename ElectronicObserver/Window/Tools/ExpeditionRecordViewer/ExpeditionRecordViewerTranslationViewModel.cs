using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.ExpeditionRecordViewer;

public class ExpeditionRecordViewerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => ExpeditionRecordViewerResources.Title;

	public string Start => DropRecordViewerResources.Start;
	public string End => DropRecordViewerResources.End;

	public string Search => SortieRecordViewerResources.Search;
	public string Cancel => GeneralRes.Cancel;

	public string Fuel => ConstantsRes.Fuel;
	public string Ammo => ConstantsRes.Ammo;
	public string Steel => ConstantsRes.Steel;
	public string Baux => ConstantsRes.Baux;

	public string Item1 => ExpeditionRecordViewerResources.Item1;
	public string Item2 => ExpeditionRecordViewerResources.Item2;

	public string FleetImage => SortieRecordViewerResources.FleetImage;
}
