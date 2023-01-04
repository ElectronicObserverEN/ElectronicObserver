﻿using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;

public class ShipTrainingPlanModel
{
	public int Id { get; set; }
	public int ShipId { get; set; }
	public int TargetLevel { get; set; }

	/// <summary>
	/// From 0 to 2
	/// </summary>
	public int TargetHPBonus { get; set; }

	/// <summary>
	/// From 0 to 9
	/// </summary>
	public int TargetASWBonus { get; set; }

	/// <summary>
	/// Targetted amount of luck 
	/// eg Yukikaze k2 max luck is 120, then i set this value to 120 to target max
	/// </summary>
	public int TargetLuck { get; set; }

	public ShipId? TargetRemodel { get; set; }
}
