namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipId;

public class ShipIdCondition : FleetCondition
{
	public Core.Types.ShipId Id { get; set; } = Core.Types.ShipId.KamikazeKai;
	public RemodelComparison Remodel { get; set; } = RemodelComparison.Any;
}