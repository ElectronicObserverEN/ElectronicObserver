using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers;

// prefix with Form so it doesn't clash with the wpf versions
public class FormBrowserHostViewModel : WinformsHostViewModel
{
	public FormBrowserHostTranslationViewModel FormBrowserHost { get; }

	public FormBrowserHostViewModel() : base("Browser", "Browser", IconContent.FormBrowser)
	{
		FormBrowserHost = Ioc.Default.GetService<FormBrowserHostTranslationViewModel>()!;

		Title = FormBrowserHost.Title;
		FormBrowserHost.PropertyChanged += (_, _) => Title = FormBrowserHost.Title;

		// todo remove parameter cause it's never used
		WinformsControl = new FormBrowserHost(null!) { TopLevel = false };

		WindowsFormsHost.Child = WinformsControl;
	}
}
