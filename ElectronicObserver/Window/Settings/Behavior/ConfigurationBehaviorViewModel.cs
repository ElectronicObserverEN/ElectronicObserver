using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Data.DiscordRPC;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Dialog.ShipPicker;

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

	public RpcIconKind RpcIconKind { get; set; }

	public IShipDataMaster? ShipUsedForRpcIcon { get; set; }
	public ShipPickerViewModel ShipPickerViewModel { get; }

	public string SelectedShipName => ShipUsedForRpcIcon switch
	{
		{ } => ShipUsedForRpcIcon.NameEN,
		_ => Translation.Control_DiscordRpc_NoShip,
	};

	public ConfigurationBehaviorViewModel(Configuration.ConfigurationData.ConfigControl config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationBehaviorTranslationViewModel>();
		ShipPickerViewModel = Ioc.Default.GetRequiredService<ShipPickerViewModel>();

		Config = config;
		Load(config);
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
		ShipUsedForRpcIcon = config.ShipUsedForRpcIcon switch
		{
			ShipId id => KCDatabase.Instance.MasterShips[(int)id],
			_ => null,
		};
		RpcIconKind = config.RpcIconKind;
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
		Config.RpcIconKind = RpcIconKind;
		Config.ShipUsedForRpcIcon = ShipUsedForRpcIcon?.ShipId;
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

	[RelayCommand]
	private void SelectRpcIconKind(RpcIconKind? kind)
	{
		if (kind is not { } kindNotNull) return;

		RpcIconKind = kindNotNull;
	}

	[RelayCommand]
	private void OpenShipPicker()
	{
		RpcIconKind = RpcIconKind.Ship;

		ShipPickerView shipPicker = new(ShipPickerViewModel);

		if (shipPicker.ShowDialog() is true)
		{
			ShipUsedForRpcIcon = shipPicker.PickedShip;
		}
	}
}
