using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.Combat;

public class ConfigurationCombatViewModel : ConfigurationViewModelBase
{
	public ConfigurationCombatTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormBattle Config { get; }

	public bool IsScrollable { get; set; }

	public bool HideDuringBattle { get; set; }

	public bool ShowHPBar { get; set; }

	public bool ShowShipTypeInHPBar { get; set; }

	public bool Display7thAsSingleLine { get; set; }

	public ConfigurationCombatViewModel(Configuration.ConfigurationData.ConfigFormBattle config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationCombatTranslationViewModel>();

		Config = config;
		Load();
	}

	private void Load()
	{
		IsScrollable = Config.IsScrollable;
		HideDuringBattle = Config.HideDuringBattle;
		ShowHPBar = Config.ShowHPBar;
		ShowShipTypeInHPBar = Config.ShowShipTypeInHPBar;
		Display7thAsSingleLine = Config.Display7thAsSingleLine;
	}

	public override void Save()
	{
		Config.IsScrollable = IsScrollable;
		Config.HideDuringBattle = HideDuringBattle;
		Config.ShowHPBar = ShowHPBar;
		Config.ShowShipTypeInHPBar = ShowShipTypeInHPBar;
		Config.Display7thAsSingleLine = Display7thAsSingleLine;
	}
}
