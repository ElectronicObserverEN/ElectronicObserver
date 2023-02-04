using ElectronicObserver.Properties.Window.Dialog;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.SenkaViewer;

public class SenkaViewerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => SenkaViewer.Title;

	public string Start => DialogDropRecordViewer.Start;
	public string End => DialogDropRecordViewer.End;
	public string ButtonRun => DialogDropRecordViewer.ButtonRun;

	public string Senka => SenkaViewer.Senka;
	public string NormalSenka => SenkaViewer.NormalSenka;
	public string ExtraOperationSenka => SenkaViewer.ExtraOperationSenka;
	public string QuestSenka => SenkaViewer.QuestSenka;

	public string RecordedSenka => SenkaViewer.RecordedSenka;
	public string EstimatedExtraGains => SenkaViewer.EstimatedExtraGains;
}
