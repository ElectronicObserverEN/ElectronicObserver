using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentIconTypeScrap;

public class EquipmentIconTypeScrapTaskModel : ITaskModel
{
	public EquipmentIconType IconType { get; set; } = EquipmentIconType.MainGunSmall;
	public int Count { get; set; } = 1;
}
