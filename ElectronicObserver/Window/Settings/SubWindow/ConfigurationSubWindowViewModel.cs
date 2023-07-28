using System.Collections.Generic;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Settings.SubWindow.AirBase;
using ElectronicObserver.Window.Settings.SubWindow.Arsenal;
using ElectronicObserver.Window.Settings.SubWindow.Browser;
using ElectronicObserver.Window.Settings.SubWindow.Combat;
using ElectronicObserver.Window.Settings.SubWindow.Compass;
using ElectronicObserver.Window.Settings.SubWindow.Dock;
using ElectronicObserver.Window.Settings.SubWindow.Fleet;
using ElectronicObserver.Window.Settings.SubWindow.Group;
using ElectronicObserver.Window.Settings.SubWindow.Headquarters;
using ElectronicObserver.Window.Settings.SubWindow.Json;
using ElectronicObserver.Window.Settings.SubWindow.Quest;
using ElectronicObserver.Window.Settings.SubWindow.ShipTraining;

namespace ElectronicObserver.Window.Settings.SubWindow;

public class ConfigurationSubWindowViewModel : ConfigurationViewModelBase
{
	public ConfigurationSubWindowTranslationViewModel Translation { get; }

	private Configuration.ConfigurationData Config { get; }

	public ConfigurationFleetViewModel Fleet { get; }
	public ConfigurationArsenalViewModel Arsenal { get; }
	public ConfigurationDockViewModel Dock { get; }
	public ConfigurationHeadquartersViewModel Headquarters { get; }
	public ConfigurationCompassViewModel Compass { get; }
	public ConfigurationQuestViewModel Quest { get; }
	public ConfigurationGroupViewModel Group { get; }
	public ConfigurationCombatViewModel Combat { get; }
	public ConfigurationBrowserViewModel Browser { get; }
	public ConfigurationAirBaseViewModel AirBase { get; }
	public ConfigurationJsonViewModel Json { get; }
	public ConfigurationShipTrainingViewModel ShipTraining { get; }

	private IEnumerable<ConfigurationViewModelBase> Configurations()
	{
		yield return Fleet;
		yield return Arsenal;
		yield return Dock;
		yield return Headquarters;
		yield return Compass;
		yield return Quest;
		yield return Group;
		yield return Combat;
		yield return Browser;
		yield return AirBase;
		yield return Json;
		yield return ShipTraining;
	}

	public ConfigurationSubWindowViewModel(Configuration.ConfigurationData config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationSubWindowTranslationViewModel>();

		Config = config;

		Fleet = new(Config.FormFleet);
		Arsenal = new(Config.FormArsenal);
		Dock = new(Config.FormDock);
		Headquarters = new(Config.FormHeadquarters);
		Compass = new(Config.FormCompass);
		Quest = new(Config.FormQuest);
		Group = new(Config.FormShipGroup);
		Combat = new(Config.FormBattle);
		Browser = new(Config.FormBrowser);
		AirBase = new(Config.FormBaseAirCorps);
		Json = new(Config.FormJson);
		ShipTraining = new(config.FormShipTraining);
	}

	public override void Save()
	{
		foreach (ConfigurationViewModelBase configuration in Configurations())
		{
			configuration.Save();
		}
	}
}
