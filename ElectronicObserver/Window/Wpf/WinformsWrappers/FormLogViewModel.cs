using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Wpf.WinformsHost;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers;

public class FormLogViewModel : WinformsHostViewModel
{
	public FormLogTranslationViewModel FormLog { get; }

	public FormLogViewModel() : base("Log", "Log",
		ImageSourceIcons.GetIcon(IconContent.FormLog))
	{
		FormLog = Ioc.Default.GetService<FormLogTranslationViewModel>()!;

		Title = FormLog.Title;
		FormLog.PropertyChanged += (_, _) => Title = FormLog.Title;

		// todo remove parameter cause it's never used
		WinformsControl = new FormLog(null!) { TopLevel = false };

		WindowsFormsHost.Child = WinformsControl;
	}
}
