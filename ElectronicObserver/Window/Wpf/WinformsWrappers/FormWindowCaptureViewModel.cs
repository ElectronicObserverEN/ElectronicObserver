using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Wpf.WinformsHost;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers;

public class FormWindowCaptureViewModel : WinformsHostViewModel
{
	private FormWindowCaptureTranslationViewModel FormWindowCapture { get; }

	public FormWindowCaptureViewModel(FormMainViewModel parent) : base("Window Capture", "WindowCapture",
		ImageSourceIcons.GetIcon(IconContent.FormJson))
	{
		FormWindowCapture = Ioc.Default.GetService<FormWindowCaptureTranslationViewModel>()!;

		Title = FormWindowCapture.Title;
		FormWindowCapture.PropertyChanged += (_, _) => Title = FormWindowCapture.Title;

		WinformsControl = new FormWindowCapture(parent) { TopLevel = false };

		WindowsFormsHost.Child = WinformsControl;
	}
}
