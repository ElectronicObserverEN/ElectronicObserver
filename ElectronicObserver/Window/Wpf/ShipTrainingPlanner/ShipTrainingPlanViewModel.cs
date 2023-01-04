using ElectronicObserver.Data;
using ElectronicObserverTypes;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
public class ShipTrainingPlanViewModel
{
	private ShipTrainingPlanModel Model { get; }

	public IShipData Ship { get; set; }

	public int TargetLevel { get; set; }

	public int TargetHP => (Ship.IsMarried ? Ship.MasterShip.HPMaxMarried : Ship.MasterShip.HPMax) + TargetHPBonus;

	public int TargetASW => Ship.ASWModernized + TargetASWBonus - (Ship.ASWModernized - Ship.ASWBase);

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

	public ShipTrainingPlanViewModel(ShipTrainingPlanModel model)
	{
		Model = model;

		Ship = KCDatabase.Instance.Ships[model.ShipId];

		TargetLevel = model.TargetLevel;
		TargetLuck = model.TargetLuck;
		TargetHPBonus = model.TargetHPBonus;
		TargetASWBonus = model.TargetASWBonus;
		
	}

	private void Save()
	{
		Model.TargetLevel = TargetLevel;
		Model.TargetLuck = TargetLuck;
		Model.TargetHPBonus = TargetHPBonus;
		Model.TargetASWBonus = TargetASWBonus;
	}
}
