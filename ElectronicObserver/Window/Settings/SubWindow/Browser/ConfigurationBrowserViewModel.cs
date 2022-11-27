using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.Browser;

public partial class ConfigurationBrowserViewModel : ConfigurationViewModelBase
{
	public ConfigurationBrowserTranslationViewModel Translation { get; }
	private FileService FileService { get; }
	private Configuration.ConfigurationData.ConfigFormBrowser Config { get; }

	public List<DockStyle> DockStyles { get; }
	public List<CheckBoxEnumViewModel> ScreenshotFormats { get; }
	public List<ScreenshotSaveMode> ScreenshotSaveModes { get; }

	public double ZoomRate { get; set; }

	public bool ZoomFit { get; set; }

	public string LogInPageURL { get; set; }

	public bool IsEnabled { get; set; }

	public string ScreenShotPath { get; set; }

	public ScreenshotFormat ScreenShotFormat { get; set; }

	public ScreenshotSaveMode ScreenShotSaveMode { get; set; }

	public string StyleSheet { get; set; }

	public bool IsScrollable { get; set; }

	public bool AppliesStyleSheet { get; set; }

	public bool IsDMMreloadDialogDestroyable { get; set; }

	public bool AvoidTwitterDeterioration { get; set; }

	public DockStyle ToolMenuDockStyle { get; set; }

	public bool ConfirmAtRefresh { get; set; }

	public bool HardwareAccelerationEnabled { get; set; }

	public bool PreserveDrawingBuffer { get; set; }

	public bool ForceColorProfile { get; set; }

	public bool SavesBrowserLog { get; set; }

	public bool UseGadgetRedirect { get; set; }

	public bool UseVulkanWorkaround { get; set; }

	public float Volume { get; set; }

	public bool IsMute { get; set; }

	public bool IsBrowserContextMenuEnabled { get; set; }

	public ConfigurationBrowserViewModel(Configuration.ConfigurationData.ConfigFormBrowser config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationBrowserTranslationViewModel>();
		FileService = Ioc.Default.GetRequiredService<FileService>();

		DockStyles = Enum.GetValues<DockStyle>().ToList();
		ScreenshotSaveModes = Enum.GetValues<ScreenshotSaveMode>().ToList();

		ScreenshotFormats = Enum.GetValues<ScreenshotFormat>()
			.Select(f => new CheckBoxEnumViewModel(f))
			.ToList();

		foreach (CheckBoxEnumViewModel format in ScreenshotFormats)
		{
			format.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName is not nameof(format.IsChecked)) return;
				if (sender is not CheckBoxEnumViewModel { IsChecked: true, Value: ScreenshotFormat f }) return;

				ScreenShotFormat = f;
			};
		}

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ScreenShotFormat)) return;

			foreach (CheckBoxEnumViewModel format in ScreenshotFormats)
			{
				if (format.Value is not ScreenshotFormat f) return;

				format.IsChecked = f == ScreenShotFormat;
			}
		};

		Config = config;
		Load();
	}

	private void Load()
	{
		ZoomRate = Config.ZoomRate * 100;
		ZoomFit = Config.ZoomFit;
		LogInPageURL = Config.LogInPageURL;
		IsEnabled = Config.IsEnabled;
		ScreenShotPath = Config.ScreenShotPath;
		ScreenShotFormat = (ScreenshotFormat)Config.ScreenShotFormat;
		ScreenShotSaveMode = (ScreenshotSaveMode)Config.ScreenShotSaveMode;
		StyleSheet = Config.StyleSheet;
		IsScrollable = Config.IsScrollable;
		AppliesStyleSheet = Config.AppliesStyleSheet;
		IsDMMreloadDialogDestroyable = Config.IsDMMreloadDialogDestroyable;
		AvoidTwitterDeterioration = Config.AvoidTwitterDeterioration;
		ToolMenuDockStyle = Config.IsToolMenuVisible switch
		{
			false => DockStyle.Hidden,

			_ => Config.ToolMenuDockStyle switch
			{
				System.Windows.Forms.DockStyle.Top => DockStyle.Top,
				System.Windows.Forms.DockStyle.Bottom => DockStyle.Bottom,
				System.Windows.Forms.DockStyle.Left => DockStyle.Left,
				System.Windows.Forms.DockStyle.Right => DockStyle.Right,
				_ => DockStyle.Top,
			},
		};
		ConfirmAtRefresh = Config.ConfirmAtRefresh;
		HardwareAccelerationEnabled = Config.HardwareAccelerationEnabled;
		PreserveDrawingBuffer = Config.PreserveDrawingBuffer;
		ForceColorProfile = Config.ForceColorProfile;
		SavesBrowserLog = Config.SavesBrowserLog;
		UseGadgetRedirect = Config.UseGadgetRedirect;
		UseVulkanWorkaround = Config.UseVulkanWorkaround;
		Volume = Config.Volume;
		IsMute = Config.IsMute;
		IsBrowserContextMenuEnabled = Config.IsBrowserContextMenuEnabled;
	}

	public override void Save()
	{
		Config.ZoomRate = ZoomRate / 100;
		Config.ZoomFit = ZoomFit;
		Config.LogInPageURL = LogInPageURL;
		Config.IsEnabled = IsEnabled;
		Config.ScreenShotPath = ScreenShotPath;
		Config.ScreenShotFormat = (int)ScreenShotFormat;
		Config.ScreenShotSaveMode = (int)ScreenShotSaveMode;
		Config.StyleSheet = StyleSheet;
		Config.IsScrollable = IsScrollable;
		Config.AppliesStyleSheet = AppliesStyleSheet;
		Config.IsDMMreloadDialogDestroyable = IsDMMreloadDialogDestroyable;
		Config.AvoidTwitterDeterioration = AvoidTwitterDeterioration;
		Config.ToolMenuDockStyle = ToolMenuDockStyle switch
		{
			DockStyle.Top => System.Windows.Forms.DockStyle.Top,
			DockStyle.Bottom => System.Windows.Forms.DockStyle.Bottom,
			DockStyle.Left => System.Windows.Forms.DockStyle.Left,
			DockStyle.Right => System.Windows.Forms.DockStyle.Right,
			_ => Config.ToolMenuDockStyle,
		};
		Config.IsToolMenuVisible = ToolMenuDockStyle is not DockStyle.Hidden;
		Config.ConfirmAtRefresh = ConfirmAtRefresh;
		Config.HardwareAccelerationEnabled = HardwareAccelerationEnabled;
		Config.PreserveDrawingBuffer = PreserveDrawingBuffer;
		Config.ForceColorProfile = ForceColorProfile;
		Config.SavesBrowserLog = SavesBrowserLog;
		Config.UseGadgetRedirect = UseGadgetRedirect;
		Config.UseVulkanWorkaround = UseVulkanWorkaround;
		Config.Volume = Volume;
		Config.IsMute = IsMute;
		Config.IsBrowserContextMenuEnabled = IsBrowserContextMenuEnabled;
	}

	[RelayCommand]
	private void SelectScreenshotFolder()
	{
		string? newSaveDataPath = FileService.SelectFolder(ScreenShotPath);

		if (newSaveDataPath is null) return;

		ScreenShotPath = newSaveDataPath;
	}
}
