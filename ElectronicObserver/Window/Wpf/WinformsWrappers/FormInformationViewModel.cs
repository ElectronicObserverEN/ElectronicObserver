using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Wpf.WinformsHost;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers;

public class FormInformationViewModel : WinformsHostViewModel
{
	public FormInformationTranslationViewModel FormInformation { get; }

	public FormInformationViewModel() : base("Info", "Information",
		ImageSourceIcons.GetIcon(IconContent.FormInformation))
	{
		FormInformation = Ioc.Default.GetService<FormInformationTranslationViewModel>()!;

		Title = FormInformation.Title;
		FormInformation.PropertyChanged += (_, _) => Title = FormInformation.Title;

		// todo remove parameter cause it's never used
		WinformsControl = new FormInformation(null!) { TopLevel = false };

		WindowsFormsHost.Child = WinformsControl;
	}
}
