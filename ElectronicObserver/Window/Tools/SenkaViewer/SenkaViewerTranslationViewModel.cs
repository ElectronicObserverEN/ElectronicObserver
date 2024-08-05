using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.SenkaViewer;

public class SenkaViewerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => SenkaViewerResources.Title;

	public string Start => DropRecordViewerResources.Start;
	public string End => DropRecordViewerResources.End;
	public string ButtonRun => DropRecordViewerResources.ButtonRun;
	public string Cancel => GeneralRes.Cancel;

	public string Senka => SenkaViewerResources.Senka;
	public string NormalSenka => SenkaViewerResources.NormalSenka;
	public string ExtraOperationSenka => SenkaViewerResources.ExtraOperationSenka;
	public string QuestSenka => SenkaViewerResources.QuestSenka;

	public string RecordedSenka => SenkaViewerResources.RecordedSenka;
	public string EstimatedExtraGains => SenkaViewerResources.EstimatedExtraGains;
}
