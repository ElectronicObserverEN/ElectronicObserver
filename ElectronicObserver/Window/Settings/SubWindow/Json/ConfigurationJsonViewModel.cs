using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.Json;

public class ConfigurationJsonViewModel : ConfigurationViewModelBase
{
	public ConfigurationJsonTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormJson Config { get; }

	public bool AutoUpdate { get; set; }

	public bool UpdatesTree { get; set; }

	public string? AutoUpdateFilter { get; set; }

	public ConfigurationJsonViewModel(Configuration.ConfigurationData.ConfigFormJson config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationJsonTranslationViewModel>();

		Config = config;
		Load();
	}

	public void Load()
	{
		AutoUpdate = Config.AutoUpdate;
		UpdatesTree = Config.UpdatesTree;
		AutoUpdateFilter = Config.AutoUpdateFilter;
	}

	public override void Save()
	{
		Config.AutoUpdate = AutoUpdate;
		Config.UpdatesTree = UpdatesTree;
		Config.AutoUpdateFilter = AutoUpdateFilter;
	}
}
