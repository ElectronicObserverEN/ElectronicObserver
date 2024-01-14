using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Database;
using ElectronicObserver.Database.DataMigration;
using ElectronicObserver.Services;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class SortieCostViewModel
{
	public DateTime Time { get; }
	public int World { get; }
	public int Map { get; }

	private int SortieFleetId { get; }
	private bool IsCombinedFleet { get; }
	private int NodeSupportFleetId { get; }
	private int BossSupportFleetId { get; }

	private List<IFleetData?> FleetsBeforeSortie { get; }
	private List<IFleetData?>? FleetsAfterSortie { get; }
	private List<IBaseAirCorpsData> AirBases { get; }

	public SortieCostModel SortieFleetSupplyCost { get; }
	public SortieCostModel SortieFleetRepairCost { get; }
	public SortieCostModel NodeSupportSupplyCost { get; }
	public SortieCostModel BossSupportSupplyCost { get; }
	public SortieCostModel TotalSupplyCost { get; }
	public SortieCostModel TotalRepairCost { get; }
	public SortieCostModel TotalAirBaseSortieCost { get; }
	public SortieCostModel TotalAirBaseSupplyCost { get; }
	public SortieCostModel TotalCost { get; }

	public SortieCostViewModel(ElectronicObserverContext db, ToolService toolService,
		SortieRecordMigrationService sortieRecordMigrationService, SortieRecordViewModel sortie)
	{
		Time = sortie.SortieStart.ToUniversalTime();
		World = sortie.World;
		Map = sortie.Map;

		SupplyCostCalculator supplyCostCalculator = new(db, toolService, sortie);
		RepairCostCalculator repairCostCalculator = new(db, toolService, sortie);
		AirBaseCostCalculator airBaseCostCalculator = new(db, sortie);

		SortieFleetId = sortie.Model.FleetData.FleetId;
		IsCombinedFleet = sortie.Model.FleetData.CombinedFlag > 0;
		NodeSupportFleetId = sortie.Model.FleetData.NodeSupportFleetId;
		BossSupportFleetId = sortie.Model.FleetData.BossSupportFleetId;

		sortieRecordMigrationService.Migrate(db, sortie.Model).Wait();

		FleetsBeforeSortie = sortie.Model.FleetData.MakeFleets();
		FleetsAfterSortie = sortie.Model.FleetAfterSortieData.MakeFleets();
		AirBases = sortie.Model.FleetData.AirBases
			.Select(a => a.MakeAirBase())
			.ToList();

		SortieFleetSupplyCost = supplyCostCalculator.SupplyCost(FleetsBeforeSortie, FleetsAfterSortie, SortieFleetId);
		SortieFleetRepairCost = repairCostCalculator.RepairCost(FleetsBeforeSortie, FleetsAfterSortie, SortieFleetId);

		if (IsCombinedFleet)
		{
			SortieFleetSupplyCost += supplyCostCalculator.SupplyCost(FleetsBeforeSortie, FleetsAfterSortie, 2);
			SortieFleetRepairCost += repairCostCalculator.RepairCost(FleetsBeforeSortie, FleetsAfterSortie, 2);
		}

		NodeSupportSupplyCost = supplyCostCalculator.SupportSupplyCost(FleetsBeforeSortie, FleetsAfterSortie, NodeSupportFleetId);
		BossSupportSupplyCost = supplyCostCalculator.SupportSupplyCost(FleetsBeforeSortie, FleetsAfterSortie, BossSupportFleetId);

		TotalSupplyCost = SortieFleetSupplyCost + NodeSupportSupplyCost + BossSupportSupplyCost;
		TotalRepairCost = SortieFleetRepairCost;

		TotalAirBaseSortieCost = AirBases
			.Where(a => a.ActionKind is AirBaseActionKind.Mission)
			.Select(airBaseCostCalculator.AirBaseSortieCost)
			.Aggregate(new SortieCostModel(), (a, b) => a + b);

		TotalAirBaseSupplyCost = airBaseCostCalculator.AirBaseSupplyCost();

		TotalCost = TotalSupplyCost + TotalRepairCost + TotalAirBaseSortieCost + TotalAirBaseSupplyCost;
	}
}
