using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Mapinfo;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class SortieCostViewModel
{
	private ElectronicObserverContext Db { get; }

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

	public SortieCostViewModel(ElectronicObserverContext db, SortieRecordViewModel sortie, SortieDetailViewModel? sortieDetails)
	{
		Db = db;

		Time = sortie.SortieStart.ToUniversalTime();
		World = sortie.World;
		Map = sortie.Map;

		SortieFleetId = sortie.Model.FleetData.FleetId;
		IsCombinedFleet = sortie.Model.FleetData.CombinedFlag > 0;
		NodeSupportFleetId = sortie.Model.FleetData.NodeSupportFleetId;
		BossSupportFleetId = sortie.Model.FleetData.BossSupportFleetId;

		FleetsBeforeSortie = sortie.Model.FleetData.MakeFleets();
		FleetsAfterSortie = sortie.Model.FleetAfterSortieData.MakeFleets();
		AirBases = sortie.Model.FleetData.AirBases
			.Select(a => GameDataExtensions.MakeAirBase(a))
			.ToList();

		if (FleetsAfterSortie is not null)
		{
			BattleFleets? initialState = sortieDetails?.Nodes
				.OfType<BattleNode>()
				.FirstOrDefault()
				?.FirstBattle
				.FleetsBeforeBattle;

			if (initialState?.Fleets is not null)
			{
				FixHp(FleetsBeforeSortie, initialState.Fleets);
			}

			BattleFleets? finalState = sortieDetails?.Nodes
				.OfType<BattleNode>()
				.LastOrDefault()
				?.LastBattle
				.FleetsAfterBattle;

			if (finalState?.Fleets is not null)
			{
				FixHp(FleetsAfterSortie, finalState.Fleets);
			}

			SortieFleetSupplyCost = SupplyCost(FleetsBeforeSortie[SortieFleetId - 1], FleetsAfterSortie[SortieFleetId - 1]);
			SortieFleetRepairCost = RepairCost(FleetsBeforeSortie[SortieFleetId - 1], FleetsAfterSortie[SortieFleetId - 1]);

			if (IsCombinedFleet)
			{
				SortieFleetSupplyCost += SupplyCost(FleetsBeforeSortie[1], FleetsAfterSortie[1]);
				SortieFleetRepairCost += RepairCost(FleetsBeforeSortie[1], FleetsAfterSortie[1]);
			}

			NodeSupportSupplyCost = NodeSupportFleetId switch
			{
				<= 0 => new(),
				int id => SupplyCost(FleetsBeforeSortie[id - 1], FleetsAfterSortie[id - 1]),
			};

			BossSupportSupplyCost = BossSupportFleetId switch
			{
				<= 0 => new(),
				int id => SupplyCost(FleetsBeforeSortie[id - 1], FleetsAfterSortie[id - 1]),
			};

			TotalSupplyCost = SortieFleetSupplyCost + NodeSupportSupplyCost + BossSupportSupplyCost;

			TotalRepairCost = FleetsBeforeSortie
				.Zip(FleetsAfterSortie, RepairCost)
				.Aggregate(new SortieCostModel(), (a, b) => a + b);

			TotalAirBaseSortieCost = AirBases
				.Where(a => a.ActionKind is AirBaseActionKind.Mission)
				.Select(AirBaseSortieCost)
				.Aggregate(new SortieCostModel(), (a, b) => a + b);

			TotalAirBaseSupplyCost = AirBaseSupplyCost();

			TotalCost = TotalSupplyCost + TotalRepairCost + TotalAirBaseSortieCost + TotalAirBaseSupplyCost;
		}

		SortieFleetSupplyCost ??= new();
		SortieFleetRepairCost ??= new();
		NodeSupportSupplyCost ??= new();
		BossSupportSupplyCost ??= new();
		TotalSupplyCost ??= new();
		TotalRepairCost ??= new();
		TotalAirBaseSortieCost ??= new();
		TotalAirBaseSupplyCost ??= new();
		TotalCost ??= new();
	}

	private static void FixHp(List<IFleetData?> fleetsToFix, List<IFleetData?> computedFleets)
	{
		foreach ((IFleetData? dbFleet, IFleetData? computedFleet) in fleetsToFix.Zip(computedFleets))
		{
			if (dbFleet is null) continue;
			if (computedFleet is null) continue;

			foreach ((IShipData? dbShip, IShipData? computedShip) in dbFleet.MembersInstance.Zip(computedFleet.MembersInstance))
			{
				if (dbShip is null) continue;
				if (computedShip is null) continue;

				((ShipDataMock)dbShip).HPCurrent = ((ShipDataMock)computedShip).HPCurrent;
			}
		}
	}

	private static SortieCostModel SupplyCost(IFleetData? before, IFleetData? after) => (before, after) switch
	{
		(not null, not null) => before.MembersInstance
			.Zip(after.MembersInstance, SupplyCost)
			.Aggregate(new SortieCostModel(), (a, b) => a + b),

		_ => new(),
	};

	private static SortieCostModel SupplyCost(IShipData? before, IShipData? after) => (before, after) switch
	{
		(not null, not null) => new()
		{
			Fuel = SupplyCost(before, before.FuelMax, before.Fuel, after.Fuel),
			Ammo = SupplyCost(before, before.AmmoMax, before.Ammo, after.Ammo),
			Bauxite = (before.Aircraft.Sum() - after.Aircraft.Sum()) * 5,
		},

		_ => new(),
	};

	private static int SupplyCost(IShipData ship, int max, int before, int after) => (before == max) switch
	{
		true => MarriageResupply(ship, before - after),
		_ => MarriageResupply(ship, max - after) - MarriageResupply(ship, max - before),
	};

	private static int MarriageResupply(IShipData ship, int resupply) => resupply switch
	{
		<= 0 => 0,
		_ when ship.IsMarried => Math.Max(1, (int)(resupply * 0.85)),
		_ => resupply,
	};

	private static SortieCostModel RepairCost(IFleetData? before, IFleetData? after) => (before, after) switch
	{
		(not null, not null) => before.MembersInstance
			.Zip(after.MembersInstance, RepairCost)
			.Aggregate(new SortieCostModel(), (a, b) => a + b),

		_ => new(),
	};

	private static SortieCostModel RepairCost(IShipData? before, IShipData? after) => (before, after) switch
	{
		(not null, not null) => RepairCost(before, before.HPCurrent - after.HPCurrent),

		_ => new(),
	};

	private static SortieCostModel RepairCost(IShipData ship, int damage) => new()
	{
		Fuel = (int)(ship.MasterShip.Fuel * 0.032 * damage),
		Steel = (int)(ship.MasterShip.Fuel * 0.06 * damage),
	};

	private static SortieCostModel AirBaseSortieCost(IBaseAirCorpsData airBase)
		=> airBase.Squadrons.Values
			.Where(s => s.EquipmentInstance is not null)
			.Select(s => new SortieCostModel
			{
				Fuel = GetAirBasePlaneCostCategory(s.EquipmentInstance!) switch
				{
					AirBasePlaneCostCategory.AirBaseAttacker => (int)Math.Ceiling(1.5 * s.AircraftCurrent),
					AirBasePlaneCostCategory.LargePlane => 2 * s.AircraftCurrent,
					AirBasePlaneCostCategory.Other => s.AircraftCurrent,

					_ => throw new NotImplementedException(),
				},
				Ammo = GetAirBasePlaneCostCategory(s.EquipmentInstance!) switch
				{
					AirBasePlaneCostCategory.AirBaseAttacker => (int)(0.7 * s.AircraftCurrent),
					AirBasePlaneCostCategory.LargePlane => 2 * s.AircraftCurrent,
					AirBasePlaneCostCategory.Other => (int)Math.Ceiling(0.6 * s.AircraftCurrent),

					_ => throw new NotImplementedException(),
				},
			})
			.Aggregate(new SortieCostModel(), (a, b) => a + b);

	private static AirBasePlaneCostCategory GetAirBasePlaneCostCategory(IEquipmentData equip)
		=> equip.MasterEquipment.CategoryType switch
		{
			EquipmentTypes.LandBasedAttacker => AirBasePlaneCostCategory.AirBaseAttacker,
			EquipmentTypes.HeavyBomber => AirBasePlaneCostCategory.LargePlane,
			_ => AirBasePlaneCostCategory.Other,
		};

	private SortieCostModel AirBaseSupplyCost()
	{
		if (TryGetAirBaseState(Db, Time) is List<ApiAirBase> airBaseState)
		{
			if (TryGetCostFromState(AirBases, airBaseState) is SortieCostModel sortieCost)
			{
				return sortieCost;
			}
		}

		// todo: get plane shotdown from battle details
		return new();
	}

	private static List<ApiAirBase>? TryGetAirBaseState(ElectronicObserverContext db, DateTime sortieStart)
	{
		ApiFile? a = db.ApiFiles
			.Where(f => f.TimeStamp > sortieStart)
			.Where(f => f.ApiFileType == ApiFileType.Response)
			.Where(f => f.Name.Contains("api_get_member/mapinfo"))
			.OrderBy(f => f.Id)
			.FirstOrDefault();

		if (a is null) return null;

		try
		{
			ApiResponse<ApiGetMemberMapinfoResponse>? response = JsonSerializer
				.Deserialize<ApiResponse<ApiGetMemberMapinfoResponse>>(a.Content);

			return response?.ApiData.ApiAirBase;
		}
		catch
		{
			// todo: log?
		}

		return null;
	}

	private static SortieCostModel? TryGetCostFromState(List<IBaseAirCorpsData> airBases, List<ApiAirBase> airBaseStates)
	{
		if (!AreIdentical(airBases, airBaseStates)) return null;

		if (airBases.Count is 0) return new();

		int world = airBases[0].MapAreaID;

		int aircraftLost = airBases
			.Zip(airBaseStates.Where(s => s.ApiAreaId == world), (ab, s) => (AirBase: ab, State: s))
			.SelectMany(t => t.AirBase.Squadrons.Values.Zip(t.State.ApiPlaneInfo, (sq, s) => (Squadron: sq, State: s)))
			.Sum(t => t.State.ApiCount switch
			{
				// null when no plane was added in the ab slot
				int count => t.Squadron.AircraftCurrent - count,
				null => 0,
			});

		return new()
		{
			Fuel = 3 * aircraftLost,
			Ammo = 5 * aircraftLost,
		};
	}

	private static bool AreIdentical(List<IBaseAirCorpsData> airBases, List<ApiAirBase> airBaseStates)
	{
		if (airBases.Count is 0) return true;

		int world = airBases[0].MapAreaID;

		foreach ((IBaseAirCorpsData airBase, ApiAirBase state) in airBases
			.Zip(airBaseStates.Where(s => s.ApiAreaId == world)))
		{
			if (airBase.ActionKind != state.ApiActionKind) return false;

			foreach ((IBaseAirCorpsSquadron squadron, ApiPlaneInfo plane) in airBase.Squadrons.Values.Zip(state.ApiPlaneInfo))
			{
				if (squadron.EquipmentMasterID != plane.ApiSlotid)
				{
					// don't have that data currently
					// return false;
				}
			}
		}

		return true;
	}
}
