using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;

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

	public string ElectronicObserverApiUrl { get; set; }

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
		ElectronicObserverApiUrl = config.ElectronicObserverApiUrl;
	}

	public override void Save()
	{
		Config.EnableDebugMenu = EnableDebugMenu;
		Config.LoadAPIListOnLoad = LoadAPIListOnLoad;
		Config.APIListPath = APIListPath;
		Config.AlertOnError = AlertOnError;
		Config.ElectronicObserverApiUrl = ElectronicObserverApiUrl;
	}

	[RelayCommand]
	private void SelectApiListPath()
	{
		string? newPath = FileService.OpenApiListPath(APIListPath);

		if (newPath is null) return;

		APIListPath = newPath;
	}
}
