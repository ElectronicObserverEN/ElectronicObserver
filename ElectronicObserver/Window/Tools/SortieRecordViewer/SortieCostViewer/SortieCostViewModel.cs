using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Database;
using ElectronicObserver.Database.DataMigration;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
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

	public SortieCostModel ResourceGain { get; }
	public SortieCostModel SinkingResourceGain { get; }

	public SortieCostModel TotalCost { get; }

	public SortieCostViewModel(ElectronicObserverContext db, ToolService toolService,
		SortieRecordMigrationService sortieRecordMigrationService, SortieRecordViewModel sortie)
	{
		Time = sortie.SortieStart.ToUniversalTime();
		World = sortie.World;
		Map = sortie.Map;

		SupplyCostCalculator supplyCostCalculator = new(db, toolService, sortie);
		RepairCostCalculator repairCostCalculator = new(db, toolService, sortie);
		AirBaseCostCalculator airBaseCostCalculator = new(db, toolService, sortie);

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

		SortieFleetSupplyCost = supplyCostCalculator.SupplyCost(FleetsBeforeSortie, FleetsAfterSortie, SortieFleetId, IsCombinedFleet);
		SortieFleetRepairCost = repairCostCalculator.RepairCost(FleetsBeforeSortie, FleetsAfterSortie, SortieFleetId, IsCombinedFleet);

		NodeSupportSupplyCost = supplyCostCalculator.NodeSupportSupplyCost(FleetsBeforeSortie, FleetsAfterSortie, NodeSupportFleetId);
		BossSupportSupplyCost = supplyCostCalculator.BossSupportSupplyCost(FleetsBeforeSortie, FleetsAfterSortie, BossSupportFleetId);

		TotalSupplyCost = SortieFleetSupplyCost + NodeSupportSupplyCost + BossSupportSupplyCost;
		TotalRepairCost = SortieFleetRepairCost;

		TotalAirBaseSortieCost = airBaseCostCalculator.AirBaseSortieCost(AirBases);
		TotalAirBaseSupplyCost = airBaseCostCalculator.AirBaseSupplyCost(AirBases);

		ResourceGain = GetResourceGain(db, toolService, sortie);
		SinkingResourceGain = new();

		TotalCost = TotalSupplyCost + TotalRepairCost + TotalAirBaseSortieCost + TotalAirBaseSupplyCost;
		TotalCost -= ResourceGain - SinkingResourceGain;
	}

	private static SortieCostModel GetResourceGain(ElectronicObserverContext db, ToolService toolService,
		SortieRecordViewModel sortie)
	{
		SortieDetailViewModel? sortieDetails = toolService.GenerateSortieDetailViewModel(db, sortie.Model);

		if (sortieDetails is null) return new();

		return sortieDetails.Nodes
			.Where(n => n.Items is not null)
			.SelectMany(n => n.Items!)
			.Select(i => GetItemId(i) switch
			{
				UseItemId.Fuel => new SortieCostModel { Fuel = i.ApiGetcount },
				UseItemId.Ammo => new SortieCostModel { Ammo = i.ApiGetcount },
				UseItemId.Steel => new SortieCostModel { Steel = i.ApiGetcount },
				UseItemId.Bauxite => new SortieCostModel { Bauxite = i.ApiGetcount },

				// todo: other items
				_ => new(),
			})
			.Sum();
	}

	private static UseItemId GetItemId(ApiItemget item) => item.ApiUsemst switch
	{
		4 => item.ApiId switch
		{
			1 => UseItemId.Fuel,
			2 => UseItemId.Ammo,
			3 => UseItemId.Steel,
			4 => UseItemId.Bauxite,
			5 => UseItemId.InstantConstruction,
			6 => UseItemId.InstantRepair,
			7 => UseItemId.DevelopmentMaterial,
			8 => UseItemId.ImproveMaterial,
			_ => UseItemId.Unknown,
		},

		_ => (UseItemId)item.ApiUsemst,
	};
}
