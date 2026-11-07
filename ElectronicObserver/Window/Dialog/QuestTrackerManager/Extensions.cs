using ElectronicObserver.Avalonia.Dialogs;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Avalonia.Translation;
using ElectronicObserver.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public static class Extensions
{
	public static IServiceCollection AddQuestTrackerManager2(this IServiceCollection services) => services
		.AddSingleton<ISoftwareUpdaterService, SoftwareUpdaterService>()
		.AddSingleton<IEoLogger, EoLogger>()
		.AddSingleton<DataService>()
		.AddSingleton<ConditionDisplayerService>()
		.AddSingleton<TaskDisplayerService>()
		.AddSingleton<FleetEvaluationService>()
		.AddSingleton<IQuestProvider, QuestProviderAdapter>()
		.AddSingleton<DialogServiceBase, WpfDialogService>()
		.AddSingleton<TaskFactory>()
		.AddSingleton<ConditionFactory>()
		.AddSingleton<QuestTrackerManager2ViewModel>()
		.AddSingleton<SystemQuestTrackerManager2>();
}
