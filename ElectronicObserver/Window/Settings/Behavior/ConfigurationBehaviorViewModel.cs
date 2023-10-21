using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Settings.Behavior;

public partial class ConfigurationBehaviorViewModel : ConfigurationViewModelBase
{
	public ConfigurationBehaviorTranslationViewModel Translation { get; }

	private Configuration.ConfigurationData.ConfigControl Config { get; }

	public int ConditionBorder { get; set; }

	public RecordAutoSaving RecordAutoSaving { get; set; }

	public bool UseSystemVolume { get; set; }

	public EngagementType PowerEngagementForm { get; set; }

	public bool ShowSallyAreaAlertDialog { get; set; }

	public bool ShowExpeditionAlertDialog { get; set; }

	public bool EnableDiscordRPC { get; set; }

	public string DiscordRPCMessage { get; set; }

	public bool DiscordRPCShowFCM { get; set; }

	public string DiscordRPCApplicationId { get; set; }

	public Uri UpdateRepoURL { get; set; }

	public bool UseFlagshipIconForRPC { get; set; }

	public bool? SubmitDataToTsunDb { get; set; }

	public ConfigurationBehaviorViewModel(Configuration.ConfigurationData.ConfigControl config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationBehaviorTranslationViewModel>();

		Config = config;
		Load(config);

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SubmitDataToTsunDb)) return;
			if (SubmitDataToTsunDb is false) return;

			// --- ask for confirmation
			DialogTsunDb dialogTsunDb = new();
			dialogTsunDb.FormClosed += (sender, e) =>
			{
				if (sender is not DialogTsunDb dialog) return;

				SubmitDataToTsunDb = dialog.DialogResult == System.Windows.Forms.DialogResult.Yes;
			};
			dialogTsunDb.ShowDialog(App.Current!.MainWindow!);
		};
	}

	private void Load(Configuration.ConfigurationData.ConfigControl config)
	{
		ConditionBorder = config.ConditionBorder;
		RecordAutoSaving = (RecordAutoSaving)config.RecordAutoSaving;
		UseSystemVolume = config.UseSystemVolume;
		PowerEngagementForm = (EngagementType)config.PowerEngagementForm;
		ShowSallyAreaAlertDialog = config.ShowSallyAreaAlertDialog;
		ShowExpeditionAlertDialog = config.ShowExpeditionAlertDialog;
		EnableDiscordRPC = config.EnableDiscordRPC;
		DiscordRPCMessage = config.DiscordRPCMessage;
		DiscordRPCShowFCM = config.DiscordRPCShowFCM;
		DiscordRPCApplicationId = config.DiscordRPCApplicationId;
		UpdateRepoURL = config.UpdateRepoURL;
		UseFlagshipIconForRPC = config.UseFlagshipIconForRPC;
		SubmitDataToTsunDb = config.SubmitDataToTsunDb;
	}

	public override void Save()
	{
		Config.ConditionBorder = ConditionBorder;
		Config.RecordAutoSaving = (int)RecordAutoSaving;
		Config.UseSystemVolume = UseSystemVolume;
		Config.PowerEngagementForm = (int)PowerEngagementForm;
		Config.ShowSallyAreaAlertDialog = ShowSallyAreaAlertDialog;
		Config.ShowExpeditionAlertDialog = ShowExpeditionAlertDialog;
		Config.EnableDiscordRPC = EnableDiscordRPC;
		Config.DiscordRPCMessage = DiscordRPCMessage;
		Config.DiscordRPCShowFCM = DiscordRPCShowFCM;
		Config.DiscordRPCApplicationId = DiscordRPCApplicationId;
		Config.UpdateRepoURL = UpdateRepoURL;
		Config.UseFlagshipIconForRPC = UseFlagshipIconForRPC;
		Config.SubmitDataToTsunDb = SubmitDataToTsunDb;
	}

	[RelayCommand]
	private void SetRecordAutoSaving(RecordAutoSaving? recordAutoSaving)
	{
		if (recordAutoSaving is not { } autoSaving) return;

		RecordAutoSaving = autoSaving;
	}

	[RelayCommand]
	private void SetPowerEngagementForm(EngagementType? engagementType)
	{
		if (engagementType is not { } engagement) return;

		PowerEngagementForm = engagement;
	}

	[RelayCommand]
	private async Task ForceUpdate()
	{
		await SoftwareUpdater.CheckUpdateAsync();
	}
}
