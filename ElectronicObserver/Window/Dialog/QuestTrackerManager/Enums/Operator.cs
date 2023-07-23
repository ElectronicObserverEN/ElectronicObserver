using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum Operator
{
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "Operator_And")]
	And,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "Operator_Or")]
	Or
}
