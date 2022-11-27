using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Behaviors.PersistentColumns;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;

namespace ElectronicObserver.Window.Settings.BGM;

public partial class ConfigurationBGMViewModel : ConfigurationViewModelBase
{
	public ConfigurationBGMTranslationViewModel Translation { get; }

	private Configuration.ConfigurationData.ConfigBGMPlayer Config { get; }

	public bool Enabled { get; set; }
	public List<SoundHandleViewModel> Handles { get; set; }
	public bool SyncBrowserMute { get; set; }
	// this volume only gets used to set the value for all SoundHandles at once
	public int SetAllVolumeValue { get; set; }

	public List<ColumnProperties> ColumnProperties { get; set; } = new();
	public List<SortDescription> SortDescriptions { get; set; } = new();

	public ConfigurationBGMViewModel(Configuration.ConfigurationData.ConfigBGMPlayer config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationBGMTranslationViewModel>();

		Config = config;
		Load();
	}

	private void Load()
	{
		Enabled = Config.Enabled;
		Handles = Config.Handles.Select(h => new SoundHandleViewModel(h)).ToList();
		SyncBrowserMute = Config.SyncBrowserMute;
		SetAllVolumeValue = (int)Handles.Average(h => h.Volume);
	}

	public override void Save()
	{
		Config.Enabled = Enabled;
		Config.Handles = Handles.Select(h => h.Handle).ToList();
		Config.SyncBrowserMute = SyncBrowserMute;
	}

	[RelayCommand]
	private void SetAllVolume()
	{
		foreach (SoundHandleViewModel handle in Handles)
		{
			handle.Volume = SetAllVolumeValue;
			handle.Save();
		}
	}

	[RelayCommand]
	private void EditSoundHandle(SoundHandleViewModel? handle)
	{
		if (handle is null) return;

		new SoundHandleEditDialog(handle).ShowDialog(App.Current!.MainWindow!);
	}
}
