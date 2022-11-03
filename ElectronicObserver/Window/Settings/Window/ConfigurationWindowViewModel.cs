using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.Window;

public partial class ConfigurationWindowViewModel : ConfigurationViewModelBase
{
	public FileService FileService { get; }
	public ConfigurationWindowTranslationViewModel Translation { get; }

	private Configuration.ConfigurationData.ConfigLife Config { get; }

	public bool ConfirmOnClosing { get; set; }

	public bool TopMost { get; set; }

	public string LayoutFilePath { get; set; }

	public bool CheckUpdateInformation { get; set; }

	public bool ShowStatusBar { get; set; }

	public ClockFormat ClockFormat { get; set; }

	public bool LockLayout { get; set; }

	public bool CanCloseFloatWindowInLock { get; set; }

	public ConfigurationWindowViewModel(Configuration.ConfigurationData.ConfigLife config)
	{
		FileService = Ioc.Default.GetRequiredService<FileService>();
		Translation = Ioc.Default.GetRequiredService<ConfigurationWindowTranslationViewModel>();

		Config = config;
		Load(config);
	}

	private void Load(Configuration.ConfigurationData.ConfigLife config)
	{
		ConfirmOnClosing = config.ConfirmOnClosing;
		TopMost = config.TopMost;
		LayoutFilePath = config.LayoutFilePath;
		CheckUpdateInformation = config.CheckUpdateInformation;
		ShowStatusBar = config.ShowStatusBar;
		ClockFormat = (ClockFormat)config.ClockFormat;
		LockLayout = config.LockLayout;
		CanCloseFloatWindowInLock = config.CanCloseFloatWindowInLock;
	}

	public override void Save()
	{
		Config.ConfirmOnClosing = ConfirmOnClosing;
		Config.TopMost = TopMost;
		Config.LayoutFilePath = LayoutFilePath;
		Config.CheckUpdateInformation = CheckUpdateInformation;
		Config.ShowStatusBar = ShowStatusBar;
		Config.ClockFormat = (int)ClockFormat;
		Config.LockLayout = LockLayout;
		Config.CanCloseFloatWindowInLock = CanCloseFloatWindowInLock;
	}

	[ICommand]
	private void SetClockFormat(ClockFormat? clockFormat)
	{
		if (clockFormat is not { } format) return;

		ClockFormat = format;
	}

	[ICommand]
	private void OpenLayout()
	{
		string? newLayoutPath = FileService.OpenLayoutPath(LayoutFilePath);

		if(newLayoutPath is null) return;

		LayoutFilePath = newLayoutPath;
	}
}
