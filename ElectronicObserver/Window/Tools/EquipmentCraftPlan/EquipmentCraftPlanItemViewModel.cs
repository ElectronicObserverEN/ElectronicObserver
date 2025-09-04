using System;
using System.Collections.Generic;
using ElectronicObserver.Common;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

namespace ElectronicObserver.Window.Tools.EquipmentCraftPlan;

public class EquipmentCraftPlanItemViewModel : WindowViewModelBase, IEquipmentPlanItemViewModel
{
	public EquipmentUpgradePlanItemViewModel? PlannedToBeUsedBy { get; set; }
	public EquipmentId EquipmentMasterDataId { get; set; }

	public EquipmentCraftPlanItemModel Model { get; set; } = new();

	public IEquipmentData? Equipment => KCDatabase.Instance.MasterEquipments.ContainsKey((int)EquipmentMasterDataId) switch
	{
		true => new EquipmentDataMock(KCDatabase.Instance.MasterEquipments[(int)EquipmentMasterDataId]),
		_ => null,
	};

	public int RequiredCount { get; set; }

	/// <summary>
	/// This is expected cost to build the required count one of this equipment
	/// </summary>
	public EquipmentUpgradePlanCostViewModel Cost { get; set; } = new(new());

	// Those are costs to do one craft attempt
	public int Fuel { get; set; }
	public int Ammo { get; set; }
	public int Steel { get; set; }
	public int Bauxite { get; set; }

	public bool Finished { get; set; }

	public int Priority { get; set; }

	public double ExpectedCraftPercentage { get; set; }

	public bool IncludeShipDropsAndQuestRewards { get; set; }
	public bool IncludeUprgadedShipDropsAndQuestRewards { get; set; }

	public List<ShipId> Ships { get; set; } = [];
	public List<ShipTypes> ShipTypes { get; set; } = [];

	public EquipmentCraftPlanItemViewModel(EquipmentId equipmentId, EquipmentUpgradePlanItemViewModel parentPlan)
	{
		PlannedToBeUsedBy = parentPlan;
		EquipmentMasterDataId = equipmentId;
	}

	public EquipmentCraftPlanItemViewModel(EquipmentCraftPlanItemModel model)
	{
		Model = model;
		LoadModel();
	}

	public void LoadModel()
	{
		EquipmentMasterDataId = Model.EquipmentId;

		PlannedToBeUsedBy?.UnsubscribeFromApis();
		PlannedToBeUsedBy = Model.Parent is null ? null : new(Model.Parent);

		Cost.UnsubscribeFromApis();
		Cost = new(new EquipmentUpgradePlanCostModel()
		{
			Fuel = (int)Math.Floor(Model.Fuel / Model.ExpectedCraftPercentage),
			Ammo = (int)Math.Floor(Model.Ammo / Model.ExpectedCraftPercentage),
			Steel = (int)Math.Floor(Model.Steel / Model.ExpectedCraftPercentage),
			Bauxite = (int)Math.Floor(Model.Bauxite / Model.ExpectedCraftPercentage),
		} * Model.RequiredCount);

		Finished = Model.Finished;
		Priority = Model.Priority;
		RequiredCount = Model.RequiredCount;
		ExpectedCraftPercentage = Model.ExpectedCraftPercentage;
		IncludeShipDropsAndQuestRewards = Model.IncludeShipDropsAndQuestRewards;
		IncludeUprgadedShipDropsAndQuestRewards = Model.IncludeUprgadedShipDropsAndQuestRewards;

		Ships = Model.Ships;
		ShipTypes = Model.ShipTypes;

		Fuel = Model.Fuel;
		Ammo = Model.Ammo;
		Steel = Model.Steel;
		Bauxite = Model.Bauxite;
	}

	public List<IEquipmentPlanItemViewModel> GetPlanChildren() => [];

	public void UnsubscribeFromApis()
	{
		Cost.UnsubscribeFromApis();
	}
}
