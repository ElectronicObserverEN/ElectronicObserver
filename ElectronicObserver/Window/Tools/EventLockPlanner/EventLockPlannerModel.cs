using System.Collections.Generic;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public class EventLockPlannerModel
{
	public int Id { get; set; }
	public List<EventLockModel> Locks { get; set; } = new();
	public List<ShipLockModel> ShipLocks { get; set; } = new();
}
