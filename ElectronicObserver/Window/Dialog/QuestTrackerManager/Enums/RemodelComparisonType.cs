using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum RemodelComparisonType
{
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "RemodelComparisonType_Any")]
	Any,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "RemodelComparisonType_AtLeast")]
	AtLeast,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "RemodelComparisonType_Exact")]
	Exact
}
