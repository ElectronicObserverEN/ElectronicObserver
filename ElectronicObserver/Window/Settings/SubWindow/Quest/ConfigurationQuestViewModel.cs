using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.Quest;

public class ConfigurationQuestViewModel : ConfigurationViewModelBase
{
	public ConfigurationQuestTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormQuest Config { get; }

	public List<AutoSavingTiming> AutoSavingTimings { get; }

	public bool ShowRunningOnly { get; set; }

	public bool ShowOnce { get; set; }

	public bool ShowDaily { get; set; }

	public bool ShowWeekly { get; set; }

	public bool ShowMonthly { get; set; }

	public bool ShowOther { get; set; }

	public int SortParameter { get; set; }

	public AutoSavingTiming ProgressAutoSaving { get; set; }

	public ConfigurationQuestViewModel(Configuration.ConfigurationData.ConfigFormQuest config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationQuestTranslationViewModel>();

		AutoSavingTimings = Enum.GetValues<AutoSavingTiming>().ToList();

		Config = config;
		Load();
	}

	private void Load()
	{
		ShowRunningOnly = Config.ShowRunningOnly;
		ShowOnce = Config.ShowOnce;
		ShowDaily = Config.ShowDaily;
		ShowWeekly = Config.ShowWeekly;
		ShowMonthly = Config.ShowMonthly;
		ShowOther = Config.ShowOther;
		SortParameter = Config.SortParameter;
		ProgressAutoSaving = (AutoSavingTiming)Config.ProgressAutoSaving;
	}

	public override void Save()
	{
		Config.ShowRunningOnly = ShowRunningOnly;
		Config.ShowOnce = ShowOnce;
		Config.ShowDaily = ShowDaily;
		Config.ShowWeekly = ShowWeekly;
		Config.ShowMonthly = ShowMonthly;
		Config.ShowOther = ShowOther;
		Config.SortParameter = SortParameter;
		Config.ProgressAutoSaving = (int)ProgressAutoSaving;
	}
}
