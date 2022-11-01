using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.Behavior;

public class ConfigurationBehaviorTranslationViewModel : TranslationBaseViewModel
{
	public string MoraleBorder => ConfigRes.MoraleBorder;

	public string LogFrequency => Properties.Window.Dialog.DialogConfiguration.LogFrequency;
	public string ProgressAutoSaving_Disable => ConfigRes.ProgressAutoSaving_Disable;
	public string ProgressAutoSaving_Hourly => ConfigRes.ProgressAutoSaving_Hourly;
	public string ProgressAutoSaving_Daily => ConfigRes.ProgressAutoSaving_Daily;
	public string ProgressAutoSaving_Immediately => Properties.Window.Dialog.DialogConfiguration.ProgressAutoSaving_Immediately;

	public string FormationModifier => Properties.Window.Dialog.DialogConfiguration.FormationModifier;
	public string Control_PowerEngagementFormToolTip => Properties.Window.Dialog.DialogConfiguration.Control_PowerEngagementFormToolTip;
	public string Control_PowerEngagementForm_Parallel => Properties.Window.Dialog.DialogConfiguration.Control_PowerEngagementForm_Parallel;
	public string Control_PowerEngagementForm_HeadOn => Properties.Window.Dialog.DialogConfiguration.Control_PowerEngagementForm_HeadOn;
	public string Control_PowerEngagementForm_GreenT => Properties.Window.Dialog.DialogConfiguration.Control_PowerEngagementForm_GreenT;
	public string Control_PowerEngagementForm_RedT => Properties.Window.Dialog.DialogConfiguration.Control_PowerEngagementForm_RedT;

	public string Control_UseSystemVolume => Properties.Window.Dialog.DialogConfiguration.Control_UseSystemVolume;
	public string Control_UseSystemVolumeToolTip => Properties.Window.Dialog.DialogConfiguration.Control_UseSystemVolumeToolTip;

	public string Control_ShowSallyAreaAlertDialog => Properties.Window.Dialog.DialogConfiguration.Control_ShowSallyAreaAlertDialog;
	public string Control_ShowSallyAreaAlertDialogTooltip => Properties.Window.Dialog.DialogConfiguration.Control_ShowSallyAreaAlertDialogTooltip;
	public string Control_ShowExpeditionAlertDialog => Properties.Window.Dialog.DialogConfiguration.Control_ShowExpeditionAlertDialog;
	public string Control_ShowExpeditionAlertDialogTooltip => Properties.Window.Dialog.DialogConfiguration.Control_ShowExpeditionAlertDialogTooltip;

	public string Control_EnableDiscordRPC => Properties.Window.Dialog.DialogConfiguration.Control_EnableDiscordRPC;
	public string Control_EnableDiscordRPCToolTip => Properties.Window.Dialog.DialogConfiguration.Control_EnableDiscordRPCToolTip;

	public string Control_DiscordRPCShowFCM => Properties.Window.Dialog.DialogConfiguration.Control_DiscordRPCShowFCM;
	public string Control_DiscordRPCShowFCMToolTip => Properties.Window.Dialog.DialogConfiguration.Control_DiscordRPCShowFCMToolTip;
	public string CheckBoxUseSecretaryIconForRPC => Properties.Window.Dialog.DialogConfiguration.checkBoxUseSecretaryIconForRPC;
	public string CheckBoxUseSecretaryIconForRPCToolTip => Properties.Window.Dialog.DialogConfiguration.checkBoxUseSecretaryIconForRPCToolTip;

	public string DiscordRPCMessage => Properties.Window.Dialog.DialogConfiguration.DiscordRPCMessage;
	public string Control_DiscordRPCMessageToolTip => Properties.Window.Dialog.DialogConfiguration.Control_DiscordRPCMessageToolTip;

	public string ApplicationId => Properties.Window.Dialog.DialogConfiguration.ApplicationId;
	public string Control_ApplicationIDToolTip => Properties.Window.Dialog.DialogConfiguration.Control_ApplicationIDToolTip;

	public string TranslationURL => Properties.Window.Dialog.DialogConfiguration.TranslationURL;
	public string Control_translationURLToolTip => Properties.Window.Dialog.DialogConfiguration.Control_translationURLToolTip;
	public string Control_ForceUpdate => Properties.Window.Dialog.DialogConfiguration.Control_ForceUpdate;

	public string Control_EnableTsunDbSubmission => Properties.Window.Dialog.DialogConfiguration.Control_EnableTsunDbSubmission;
}
