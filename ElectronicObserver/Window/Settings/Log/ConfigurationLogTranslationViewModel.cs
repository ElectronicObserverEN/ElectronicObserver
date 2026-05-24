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
	public string Log_ShowDropSpoiler => ConfigurationResources.Log_ShowDropSpoiler;
	public string Log_ShowDropSpoilerToolTip => ConfigurationResources.Log_ShowDropSpoilerToolTip;
	public string Log_ShowExpeditionSpoiler => ConfigurationResources.Log_ShowExpeditionSpoiler;
	public string Log_ShowExpeditionSpoilerToolTip => ConfigurationResources.Log_ShowExpeditionSpoilerToolTip;
	public string Log_ShowDevelopmentSpoiler => ConfigurationResources.Log_ShowDevelopmentSpoiler;
	public string Log_ShowDevelopmentSpoilerToolTip => ConfigurationResources.Log_ShowDevelopmentSpoilerToolTip;
	public string Log_ShowEquipmentImprovementSpoiler => ConfigurationResources.Log_ShowEquipmentImprovementSpoiler;
	public string Log_ShowEquipmentImprovementSpoilerToolTip => ConfigurationResources.Log_ShowEquipmentImprovementSpoilerToolTip;
	public string Log_ShowModernizationSpoiler => ConfigurationResources.Log_ShowModernizationSpoiler;
	public string Log_ShowModernizationSpoilerToolTip => ConfigurationResources.Log_ShowModernizationSpoilerToolTip;
	public string Log_ShowConstructionSpoiler => ConfigurationResources.Log_ShowConstructionSpoiler;
	public string Log_ShowConstructionSpoilerToolTip => ConfigurationResources.Log_ShowConstructionSpoilerToolTip;

	public string Log_SaveErrorReport => ConfigRes.SaveErrorReport;
	public string SaveErrorToolTip => ConfigRes.SaveErrorHint;

	public string Encoding => ConfigRes.Enocding;
	public string EncodingToolTip => ConfigRes.EncodingHint;
	public string CorruptLogs => ConfigRes.CorruptLogs;

	public string Log_SaveBattleLog => ConfigurationResources.Log_SaveBattleLog;
	public string Log_SaveBattleLogToolTip => ConfigurationResources.Log_SaveBattleLogToolTip;
}
