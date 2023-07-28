using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.Behavior;

public class ConfigurationBehaviorTranslationViewModel : TranslationBaseViewModel
{
	public string MoraleBorder => ConfigRes.MoraleBorder;

	public string LogFrequency => ConfigurationResources.LogFrequency;
	public string ProgressAutoSaving_Disable => ConfigRes.ProgressAutoSaving_Disable;
	public string ProgressAutoSaving_Hourly => ConfigRes.ProgressAutoSaving_Hourly;
	public string ProgressAutoSaving_Daily => ConfigRes.ProgressAutoSaving_Daily;
	public string ProgressAutoSaving_Immediately => ConfigurationResources.ProgressAutoSaving_Immediately;

	public string FormationModifier => ConfigurationResources.FormationModifier;
	public string Control_PowerEngagementFormToolTip => ConfigurationResources.Control_PowerEngagementFormToolTip;
	public string Control_PowerEngagementForm_Parallel => ConfigurationResources.Control_PowerEngagementForm_Parallel;
	public string Control_PowerEngagementForm_HeadOn => ConfigurationResources.Control_PowerEngagementForm_HeadOn;
	public string Control_PowerEngagementForm_GreenT => ConfigurationResources.Control_PowerEngagementForm_GreenT;
	public string Control_PowerEngagementForm_RedT => ConfigurationResources.Control_PowerEngagementForm_RedT;

	public string Control_UseSystemVolume => ConfigurationResources.Control_UseSystemVolume;
	public string Control_UseSystemVolumeToolTip => ConfigurationResources.Control_UseSystemVolumeToolTip;

	public string Control_ShowSallyAreaAlertDialog => ConfigurationResources.Control_ShowSallyAreaAlertDialog;
	public string Control_ShowSallyAreaAlertDialogTooltip => ConfigurationResources.Control_ShowSallyAreaAlertDialogTooltip;
	public string Control_ShowExpeditionAlertDialog => ConfigurationResources.Control_ShowExpeditionAlertDialog;
	public string Control_ShowExpeditionAlertDialogTooltip => ConfigurationResources.Control_ShowExpeditionAlertDialogTooltip;

	public string Control_EnableDiscordRPC => ConfigurationResources.Control_EnableDiscordRPC;
	public string Control_EnableDiscordRPCToolTip => ConfigurationResources.Control_EnableDiscordRPCToolTip;

	public string Control_DiscordRPCShowFCM => ConfigurationResources.Control_DiscordRPCShowFCM;
	public string Control_DiscordRPCShowFCMToolTip => ConfigurationResources.Control_DiscordRPCShowFCMToolTip;
	public string CheckBoxUseSecretaryIconForRPC => ConfigurationResources.checkBoxUseSecretaryIconForRPC;
	public string CheckBoxUseSecretaryIconForRPCToolTip => ConfigurationResources.checkBoxUseSecretaryIconForRPCToolTip;

	public string DiscordRPCMessage => ConfigurationResources.DiscordRPCMessage;
	public string Control_DiscordRPCMessageToolTip => ConfigurationResources.Control_DiscordRPCMessageToolTip;

	public string ApplicationId => ConfigurationResources.ApplicationId;
	public string Control_ApplicationIDToolTip => ConfigurationResources.Control_ApplicationIDToolTip;

	public string TranslationURL => ConfigurationResources.TranslationURL;
	public string Control_translationURLToolTip => ConfigurationResources.Control_translationURLToolTip;
	public string Control_ForceUpdate => ConfigurationResources.Control_ForceUpdate;

	public string Control_EnableTsunDbSubmission => ConfigurationResources.Control_EnableTsunDbSubmission;
}
