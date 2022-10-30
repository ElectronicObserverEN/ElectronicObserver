using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.AirBase;

public class ConfigurationAirBaseViewModel : ConfigurationViewModelBase
{
	public ConfigurationAirBaseTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormBaseAirCorps Config { get; }

	public bool ShowEventMapOnly { get; set; }

	public ConfigurationAirBaseViewModel(Configuration.ConfigurationData.ConfigFormBaseAirCorps config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationAirBaseTranslationViewModel>();

		Config = config;
		Load();
	}

	private void Load()
	{
		ShowEventMapOnly = Config.ShowEventMapOnly;
	}

	public override void Save()
	{
		Config.ShowEventMapOnly = ShowEventMapOnly;
	}
}
