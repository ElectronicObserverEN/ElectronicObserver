using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.Compass;

public class ConfigurationCompassViewModel : ConfigurationViewModelBase
{
	public ConfigurationCompassTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormCompass Config { get; }

	public int MaxShipNameWidth { get; set; }
	public bool DisplayAllEnemyCompositions { get; set; }

	public ConfigurationCompassViewModel(Configuration.ConfigurationData.ConfigFormCompass config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationCompassTranslationViewModel>();

		Config = config;
		Load();
	}

	private void Load()
	{
		MaxShipNameWidth = Config.MaxShipNameWidth;
		DisplayAllEnemyCompositions = Config.DisplayAllEnemyCompositions;
	}

	public override void Save()
	{
		Config.MaxShipNameWidth = MaxShipNameWidth;
		Config.DisplayAllEnemyCompositions = DisplayAllEnemyCompositions;
	}
}
