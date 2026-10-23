using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public static class TrackerViewModelExtensions
{
	public static double Progress(this ITrackerViewModel tracker) => tracker switch
	{
		TrackerViewModelV2 trackerV2 => trackerV2.Tasks
			.OfType<TaskViewModel>()
			.Select(t => (double)t.Progress / t.Count)
			.DefaultIfEmpty()
			.Average(),

		TrackerViewModel trackerOld => trackerOld.Model.Tasks
			.OfType<IQuestTask>()
			.Select(t => (double)t.Progress / t.Count)
			.DefaultIfEmpty()
			.Average(),

		_ => 0,
	};

	public static IEnumerable<string?> TaskProgressDisplays(this ITrackerViewModel tracker) => tracker switch
	{
		TrackerViewModelV2 trackerV2 => trackerV2.Tasks
			.Select(t => t switch
			{
				null => QuestTrackerManagerResources.UnknownTask,
				{ } task when task.Progress >= task.Count => "",
				{ } task => task.Display,
			})
			.Where(s => !string.IsNullOrEmpty(s)),

		TrackerViewModel trackerOld => trackerOld.Tasks
			.Select(t => t switch
			{
				null => QuestTrackerManagerResources.UnknownTask,
				_ => t.Progress,
			})
			.Where(p => !string.IsNullOrEmpty(p)),

		_ => [],
	};

	public static string ProgressDisplay(this ITrackerViewModel tracker) => tracker.TaskCount switch
	{
		0 => "-",
		_ => string.Join("\n", tracker.TaskProgressDisplays()) switch
		{
			null or "" => "100%",
			{ } s => s,
		},
	};
}
