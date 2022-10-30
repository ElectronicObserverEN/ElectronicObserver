using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.Group;

public class ConfigurationGroupViewModel : ConfigurationViewModelBase
{
	public ConfigurationGroupTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormShipGroup Config { get; }

	public List<ShipNameSortMethod> ShipNameSortMethods { get; }

	public bool AutoUpdate { get; set; }

	public bool ShowStatusBar { get; set; }

	public ShipNameSortMethod ShipNameSortMethod { get; set; }

	public ConfigurationGroupViewModel(Configuration.ConfigurationData.ConfigFormShipGroup config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationGroupTranslationViewModel>();

		ShipNameSortMethods = Enum.GetValues<ShipNameSortMethod>().ToList();

		Config = config;
		Load();
	}

	private void Load()
	{
		AutoUpdate = Config.AutoUpdate;
		ShowStatusBar = Config.ShowStatusBar;
		ShipNameSortMethod = (ShipNameSortMethod)Config.ShipNameSortMethod;
	}

	public override void Save()
	{
		Config.AutoUpdate = AutoUpdate;
		Config.ShowStatusBar = ShowStatusBar;
		Config.ShipNameSortMethod = (int)ShipNameSortMethod;
	}
}
