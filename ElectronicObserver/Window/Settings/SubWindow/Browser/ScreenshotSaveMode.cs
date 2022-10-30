using System.ComponentModel.DataAnnotations;
using ElectronicObserver.Properties.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Browser;

public enum ScreenshotSaveMode
{
	[Display(ResourceType = typeof(DialogConfiguration), Name = "FormBrowser_ScreenShotSaveMode_ToFile")]
	File = 1,

	[Display(ResourceType = typeof(DialogConfiguration), Name = "FormBrowser_ScreenShotSaveMode_ToClipboard")]
	Clipboard = 2,

	[Display(ResourceType = typeof(DialogConfiguration), Name = "FormBrowser_ScreenShotSaveMode_Both")]
	Both = 3,
}
