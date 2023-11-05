using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.Log;

public class ConfigurationLogTranslationViewModel : TranslationBaseViewModel
{
	public string Log_SaveLogFlag => ConfigRes.SaveLog;
	public string Log_SaveLogImmediately => ConfigurationResources.Log_SaveLogImmediately;
	public string Log_SaveLogImmediatelyToolTip => ConfigurationResources.Log_SaveLogImmediatelyToolTip;

	public string LoggingLevel => ConfigRes.LoggingLevel;
	public string Log_LogLevelToolTip => ConfigurationResources.Log_LogLevelToolTip;
	public string Log_ShowSpoiler => ConfigurationResources.Log_ShowSpoiler;
	public string Log_ShowSpoilerToolTip => ConfigurationResources.Log_ShowSpoilerToolTip;

	public string Log_SaveErrorReport => ConfigRes.SaveErrorReport;
	public string SaveErrorToolTip => ConfigRes.SaveErrorHint;

	public string Encoding => ConfigRes.Enocding;
	public string EncodingToolTip => ConfigRes.EncodingHint;
	public string CorruptLogs => ConfigRes.CorruptLogs;

	public string Log_SaveBattleLog => ConfigurationResources.Log_SaveBattleLog;
	public string Log_SaveBattleLogToolTip => ConfigurationResources.Log_SaveBattleLogToolTip;
}
