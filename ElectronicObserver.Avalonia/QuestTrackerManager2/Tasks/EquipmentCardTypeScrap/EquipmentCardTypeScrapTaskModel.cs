using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCardTypeScrap;

public class EquipmentCardTypeScrapTaskModel : ITaskModel
{
	public EquipmentCardType CardType { get; set; } = EquipmentCardType.PrimaryArmament;
	public int Count { get; set; } = 1;
}
