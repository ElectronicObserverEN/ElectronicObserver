using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Wpf.WinformsHost;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers;

public class FormQuestViewModel : WinformsHostViewModel
{
	public FormQuestTranslationViewModel FormQuest { get; }

	public FormQuestViewModel() : base("Quests", "Quest",
		ImageSourceIcons.GetIcon(IconContent.FormQuest))
	{
		FormQuest = Ioc.Default.GetService<FormQuestTranslationViewModel>()!;

		Title = FormQuest.Title;
		FormQuest.PropertyChanged += (_, _) => Title = FormQuest.Title;

		// todo remove parameter cause it's never used
		WinformsControl = new FormQuest(null!) { TopLevel = false };

		WindowsFormsHost.Child = WinformsControl;
	}
}
