using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

namespace ElectronicObserver.Window.Tools.EquipmentCraftPlan;

public class EquipmentCraftPlanItemModel
{
	public int Id { get; set; }

	public EquipmentUpgradePlanItemModel? Parent { get; set; }

	public EquipmentId EquipmentId { get; set; }

	public bool Finished { get; set; }

	public int Priority { get; set; }

	public int RequiredCount { get; set; }

	public int Fuel { get; set; }
	public int Ammo { get; set; }
	public int Steel { get; set; }
	public int Bauxite { get; set; }

	public double ExpectedCraftPercentage { get; set; }

	public bool IncludeShipDropsAndQuestRewards { get; set; }
	public bool IncludeUprgadedShipDropsAndQuestRewards { get; set; }

	public List<ShipId> Ships { get; set; } = [];
	public List<ShipTypes> ShipTypes { get; set; } = [];
}
