using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.Arsenal;

public class ConfigurationArsenalViewModel : ConfigurationViewModelBase
{
	public ConfigurationArsenalTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormArsenal Config { get; }

	public bool ShowShipName { get; set; }

	public bool BlinkAtCompletion { get; set; }

	public int MaxShipNameWidth { get; set; }

	public ConfigurationArsenalViewModel(Configuration.ConfigurationData.ConfigFormArsenal config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationArsenalTranslationViewModel>();

		Config = config;
		Load();
	}

	private void Load()
	{
		ShowShipName = Config.ShowShipName;
		BlinkAtCompletion = Config.BlinkAtCompletion;
		MaxShipNameWidth = Config.MaxShipNameWidth;
	}

	public override void Save()
	{
		Config.ShowShipName = ShowShipName;
		Config.BlinkAtCompletion = BlinkAtCompletion;
		Config.MaxShipNameWidth = MaxShipNameWidth;
	}
}
