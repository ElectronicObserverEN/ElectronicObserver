using ElectronicObserver.Dialogs.TextInput;
using ElectronicObserver.ViewModels;
using HanumanInstitute.MvvmDialogs.Wpf;

namespace ElectronicObserver.Dialogs;

public class ViewLocator : StrongViewLocator
{
	public ViewLocator()
	{
		Register<TextInputViewModel, TextInputDialog>();

		Register<FormMainViewModel, FormMainWpf>();
	}
}
