using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentScrap;

public class EquipmentScrapTaskModel : ITaskModel
{
	public EquipmentId Id { get; set; } = EquipmentId.MainGunSmall_12cmSingleGun;
	public int Count { get; set; } = 1;
}
