using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.Dock;

public class ConfigurationDockViewModel : ConfigurationViewModelBase
{
	public ConfigurationDockTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormDock Config { get; }

	public bool BlinkAtCompletion { get; set; }

	public int MaxShipNameWidth { get; set; }

	public ConfigurationDockViewModel(Configuration.ConfigurationData.ConfigFormDock config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationDockTranslationViewModel>();

		Config = config;
		Load();
	}

	private void Load()
	{
		BlinkAtCompletion = Config.BlinkAtCompletion;
		MaxShipNameWidth = Config.MaxShipNameWidth;
	}

	public override void Save()
	{
		Config.BlinkAtCompletion = BlinkAtCompletion;
		Config.MaxShipNameWidth = MaxShipNameWidth;
	}
}
