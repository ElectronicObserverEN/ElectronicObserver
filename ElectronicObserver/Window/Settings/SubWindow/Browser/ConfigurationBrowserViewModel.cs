using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BrowserLibCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.Browser;

public partial class ConfigurationBrowserViewModel : ConfigurationViewModelBase
{
	public ConfigurationBrowserTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormBrowser Config { get; }

	public List<CheckBoxEnumViewModel> EmbeddedBrowsers { get; }
	public List<DockStyle> DockStyles { get; }
	public List<CheckBoxEnumViewModel> ScreenshotFormats { get; }
	public List<ScreenshotSaveMode> ScreenshotSaveModes { get; }
	public List<ScreenshotMode> ScreenshotModes { get; }

	public BrowserOption Browser { get; set; }

	public bool IsEnabled { get; set; }
	public double ZoomRate { get; set; }
	public bool ZoomFit { get; set; }
	public bool ConfirmAtRefresh { get; set; }
	public bool AppliesStyleSheet { get; set; }
	public bool UseHttps { get; set; }
	public bool IsBrowserContextMenuEnabled { get; set; }
	public ScreenshotMode ScreenshotMode { get; set; }
	public string LogInPageURL { get; set; }
	public DockStyle ToolMenuDockStyle { get; set; }
	public bool IsDMMreloadDialogDestroyable { get; set; }

	public ScreenshotFormat ScreenShotFormat { get; set; }
	public bool AvoidTwitterDeterioration { get; set; }
	public ScreenshotSaveMode ScreenShotSaveMode { get; set; }
	public string ScreenShotPath { get; set; }

	public bool HardwareAccelerationEnabled { get; set; }
	public bool PreserveDrawingBuffer { get; set; }
	public bool ForceColorProfile { get; set; }
	public bool SavesBrowserLog { get; set; }
	public bool UseVulkanWorkaround { get; set; }

	public ConfigurationBrowserViewModel(Configuration.ConfigurationData.ConfigFormBrowser config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationBrowserTranslationViewModel>();

		EmbeddedBrowsers = Enum.GetValues<BrowserOption>()
			.Select(b => new CheckBoxEnumViewModel(b))
			.ToList();
		DockStyles = Enum.GetValues<DockStyle>().ToList();
		ScreenshotSaveModes = Enum.GetValues<ScreenshotSaveMode>().ToList();

		ScreenshotFormats = Enum.GetValues<ScreenshotFormat>()
			.Select(f => new CheckBoxEnumViewModel(f))
			.ToList();

		ScreenshotModes = [.. Enum.GetValues<ScreenshotMode>()];

		foreach (CheckBoxEnumViewModel browser in EmbeddedBrowsers)
		{
			browser.IsChecked = browser.Value is BrowserOption bo && bo == Browser;
			browser.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName is not nameof(browser.IsChecked)) return;
				if (sender is not CheckBoxEnumViewModel { IsChecked: true, Value: BrowserOption b }) return;

				Browser = b;
			};
		}

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Browser)) return;

			foreach (CheckBoxEnumViewModel browser in EmbeddedBrowsers)
			{
				if (browser.Value is not BrowserOption b) return;

				browser.IsChecked = b == Browser;
			}
		};

		foreach (CheckBoxEnumViewModel format in ScreenshotFormats)
		{
			format.IsChecked = format.Value is ScreenshotFormat sf && sf == ScreenShotFormat;
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
		Browser = Config.Browser;
		ZoomRate = Config.ZoomRate * 100;
		ZoomFit = Config.ZoomFit;
		LogInPageURL = Config.LogInPageURL;
		IsEnabled = Config.IsEnabled;
		ScreenShotPath = Config.ScreenShotPath;
		ScreenShotFormat = (ScreenshotFormat)Config.ScreenShotFormat;
		ScreenShotSaveMode = (ScreenshotSaveMode)Config.ScreenShotSaveMode;
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
		UseVulkanWorkaround = Config.UseVulkanWorkaround;
		IsBrowserContextMenuEnabled = Config.IsBrowserContextMenuEnabled;
		ScreenshotMode = Config.ScreenshotMode;
	}

	public override void Save()
	{
		Config.Browser = Browser;
		Config.ZoomRate = ZoomRate / 100;
		Config.ZoomFit = ZoomFit;
		Config.LogInPageURL = LogInPageURL;
		Config.IsEnabled = IsEnabled;
		Config.ScreenShotPath = ScreenShotPath;
		Config.ScreenShotFormat = (int)ScreenShotFormat;
		Config.ScreenShotSaveMode = (int)ScreenShotSaveMode;
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
		Config.UseVulkanWorkaround = UseVulkanWorkaround;
		Config.IsBrowserContextMenuEnabled = IsBrowserContextMenuEnabled;
		Config.ScreenshotMode = ScreenshotMode;
	}

	[RelayCommand]
	private void SelectScreenshotFolder()
	{
		string? newSaveDataPath = FileService.SelectFolder(ScreenShotPath);

		if (newSaveDataPath is null) return;

		ScreenShotPath = newSaveDataPath;
	}
}
