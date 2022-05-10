using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Wpf.WinformsHost;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers;

public class FormShipGroupViewModel : WinformsHostViewModel
{
	public FormShipGroupTranslationViewModel FormShipGroup { get; }

	public FormShipGroupViewModel() : base("Group", "ShipGroup",
		ImageSourceIcons.GetIcon(IconContent.FormShipGroup))
	{
		FormShipGroup = Ioc.Default.GetService<FormShipGroupTranslationViewModel>()!;

		Title = FormShipGroup.Title;
		FormShipGroup.PropertyChanged += (_, _) => Title = FormShipGroup.Title;

		// todo remove parameter cause it's never used
		WinformsControl = new FormShipGroup(null!) { TopLevel = false };

		WindowsFormsHost.Child = WinformsControl;
	}
}
