using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCategoryScrap;

public class EquipmentCategoryScrapTaskModel : ITaskModel
{
	public EquipmentTypes Category { get; set; } = EquipmentTypes.MainGunSmall;
	public int Count { get; set; } = 1;
}
