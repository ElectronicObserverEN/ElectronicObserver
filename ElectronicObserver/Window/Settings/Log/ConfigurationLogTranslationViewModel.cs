using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.Log;

public class ConfigurationLogTranslationViewModel : TranslationBaseViewModel
{
	public string Log_SaveLogFlag => ConfigRes.SaveLog;
	public string Log_SaveLogImmediately => Properties.Window.Dialog.DialogConfiguration.Log_SaveLogImmediately;
	public string Log_SaveLogImmediatelyToolTip => Properties.Window.Dialog.DialogConfiguration.Log_SaveLogImmediatelyToolTip;

	public string LoggingLevel => ConfigRes.LoggingLevel;
	public string Log_LogLevelToolTip => Properties.Window.Dialog.DialogConfiguration.Log_LogLevelToolTip;
	public string Log_ShowSpoiler => Properties.Window.Dialog.DialogConfiguration.Log_ShowSpoiler;
	public string Log_ShowSpoilerToolTip => Properties.Window.Dialog.DialogConfiguration.Log_ShowSpoilerToolTip;

	public string Log_SaveErrorReport => ConfigRes.SaveErrorReport;
	public string SaveErrorToolTip => ConfigRes.SaveErrorHint;

	public string Encoding => ConfigRes.Enocding;
	public string EncodingToolTip => ConfigRes.EncodingHint;
	public string CorruptLogs => ConfigRes.CorruptLogs;

	public string Log_SaveBattleLog => Properties.Window.Dialog.DialogConfiguration.Log_SaveBattleLog;
	public string Log_SaveBattleLogToolTip => Properties.Window.Dialog.DialogConfiguration.Log_SaveBattleLogToolTip;
}
