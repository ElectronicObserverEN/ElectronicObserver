using System;
using System.Timers;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;

namespace ElectronicObserver.Window.Settings;

public partial class ConfigurationViewModel : WindowViewModelBase
{
	public ConfigurationTranslationViewModel Translation { get; }

	private Timer Timer { get; } = new();
	public string Log_PlayTime { get; set; } = "";
	private DateTime ShownTime { get; set; }
	private double PlayTimeCache { get; set; }

	public bool? DialogResult { get; set; }

	public ConfigurationViewModel()
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationTranslationViewModel>();

		ShownTime = DateTime.Now;
		PlayTimeCache = Utility.Configuration.Config.Log.PlayTime;
		UpdatePlayTime();

		Timer.Interval = 1000;
		Timer.Elapsed += (sender, args) =>
		{
			UpdatePlayTime();
		};

		Timer.Start();
	}

	private void UpdatePlayTime()
	{
		double elapsed = (DateTime.Now - ShownTime).TotalSeconds;
		Log_PlayTime = Utility.Mathematics.DateTimeHelper.ToTimeElapsedString(TimeSpan.FromSeconds(PlayTimeCache + elapsed));
	}

	private void SaveConfigurations()
	{

	}

	[ICommand]
	private void Confirm()
	{
		SaveConfigurations();

		DialogResult = true;
	}

	[ICommand]
	private void Cancel()
	{
		DialogResult = false;
	}
}
