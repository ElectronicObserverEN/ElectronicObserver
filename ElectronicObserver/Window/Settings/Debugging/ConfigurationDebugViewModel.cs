﻿using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.EquipmentPicker;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Settings.Debugging;

public partial class ConfigurationDebugViewModel : ConfigurationViewModelBase
{
	public ConfigurationDebugTranslationViewModel Translation { get; }
	private FileService FileService { get; }

	private Configuration.ConfigurationData.ConfigDebug Config { get; }

	// todo: when false, certain config options should be hidden
	// Connection_UpstreamProxyAddress
	// Connection_DownstreamProxy
	// Connection_DownstreamProxyLabel
	// SubWindow_Json_SealingPanel - all json config
	public bool EnableDebugMenu { get; set; }

	public bool LoadAPIListOnLoad { get; set; }

	public string APIListPath { get; set; }

	public bool AlertOnError { get; set; }

	public ConfigurationDebugViewModel(Configuration.ConfigurationData.ConfigDebug config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationDebugTranslationViewModel>();
		FileService = Ioc.Default.GetRequiredService<FileService>();

		Config = config;
		Load(config);
	}

	private void Load(Configuration.ConfigurationData.ConfigDebug config)
	{
		EnableDebugMenu = config.EnableDebugMenu;
		LoadAPIListOnLoad = config.LoadAPIListOnLoad;
		APIListPath = config.APIListPath;
		AlertOnError = config.AlertOnError;
	}

	public override void Save()
	{
		Config.EnableDebugMenu = EnableDebugMenu;
		Config.LoadAPIListOnLoad = LoadAPIListOnLoad;
		Config.APIListPath = APIListPath;
		Config.AlertOnError = AlertOnError;
	}

	[ICommand]
	private void SelectApiListPath()
	{
		string? newPath = FileService.OpenApiListPath(APIListPath);

		if (newPath is null) return;

		APIListPath = newPath;
	}


	#region Testing

	public IEquipmentData? SelectedEquipment { get; set; }

	[ICommand]
	private void OpenEquipmentPicker()
	{
		EquipmentPickerService service = Ioc.Default.GetService<EquipmentPickerService>()!;

		SelectedEquipment = service.OpenEquipmentPicker();
	}

	public IEquipmentDataMaster? SelectedMasterEquipment { get; set; }

	[ICommand]
	private void OpenMasterEquipmentPicker()
	{
		EquipmentPickerService service = Ioc.Default.GetService<EquipmentPickerService>()!;

		SelectedMasterEquipment = service.OpenMasterEquipmentPicker();
	}

	#endregion
}
