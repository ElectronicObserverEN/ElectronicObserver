using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Mapinfo;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class AirBaseCostCalculator(ElectronicObserverContext db, ToolService toolService,
	SortieRecordViewModel sortie)
{
	private ElectronicObserverContext Db { get; } = db;
	private ToolService ToolService { get; } = toolService;

	private SortieRecord SortieRecord { get; } = sortie.Model;
	private DateTime Time { get; } = sortie.SortieStart.ToUniversalTime();
	private SortieDetailViewModel? SortieDetails { get; set; }

	public SortieCostModel AirBaseSortieCost(IEnumerable<IBaseAirCorpsData> airBases)
	{
		if (SortieRecord.CalculatedSortieCost.TotalAirBaseSortieCost is not null)
		{
			return SortieRecord.CalculatedSortieCost.TotalAirBaseSortieCost;
		}

		SortieDetails ??= ToolService.GenerateSortieDetailViewModel(Db, SortieRecord);

		if (SortieDetails is null) return SortieCostModel.Zero;

		SortieRecord.CalculatedSortieCost.TotalAirBaseSortieCost = airBases
			.Zip(SortieDetails.StrikePoints, (corps, points) => (Corps: corps, StrikePoints: points))
			.Where(t => t.StrikePoints is not null)
			.Select(t => t.Corps)
			.Select(AirBaseSortieCost)
			.Sum();

		Db.Sorties.Update(SortieRecord);
		Db.SaveChanges();

		return SortieRecord.CalculatedSortieCost.TotalAirBaseSortieCost;
	}

	private static SortieCostModel AirBaseSortieCost(IBaseAirCorpsData airBase)
		=> airBase.Squadrons.Values
			.Where(s => s.EquipmentInstance is not null)
			.Select(AirBaseSquadronCost)
			.Sum();

	private static SortieCostModel AirBaseSquadronCost(IBaseAirCorpsSquadron squadron) =>
		squadron.EquipmentInstance switch
		{
			null => SortieCostModel.Zero,
			_ => new()
			{
				Fuel = GetAirBasePlaneCostCategory(squadron.EquipmentInstance) switch
				{
					AirBasePlaneCostCategory.AirBaseAttacker => (int)Math.Ceiling(1.5 * squadron.AircraftCurrent),
					AirBasePlaneCostCategory.LargePlane => 2 * squadron.AircraftCurrent,
					AirBasePlaneCostCategory.Other => squadron.AircraftCurrent,

					_ => throw new NotImplementedException(),
				},
				Ammo = GetAirBasePlaneCostCategory(squadron.EquipmentInstance) switch
				{
					AirBasePlaneCostCategory.AirBaseAttacker => (int)(0.7 * squadron.AircraftCurrent),
					AirBasePlaneCostCategory.LargePlane => 2 * squadron.AircraftCurrent,
					AirBasePlaneCostCategory.Other => (int)Math.Ceiling(0.6 * squadron.AircraftCurrent),

					_ => throw new NotImplementedException(),
				},
			},
		};

	private static AirBasePlaneCostCategory GetAirBasePlaneCostCategory(IEquipmentData equip)
		=> equip.MasterEquipment.CategoryType switch
		{
			EquipmentTypes.LandBasedAttacker => AirBasePlaneCostCategory.AirBaseAttacker,
			EquipmentTypes.HeavyBomber => AirBasePlaneCostCategory.LargePlane,
			_ => AirBasePlaneCostCategory.Other,
		};

	public SortieCostModel AirBaseSupplyCost(List<IBaseAirCorpsData> airBases)
	{
		if (SortieRecord.CalculatedSortieCost.TotalAirBaseSupplyCost is not null)
		{
			return SortieRecord.CalculatedSortieCost.TotalAirBaseSupplyCost;
		}

		SortieCostModel? airBaseSupplyCost = TryGetAirBaseSupplyCostFromDatabase(airBases);

		airBaseSupplyCost ??= CalculateAirBaseSupplyCost();

		SortieRecord.CalculatedSortieCost.TotalAirBaseSupplyCost = airBaseSupplyCost;
		Db.Sorties.Update(SortieRecord);
		Db.SaveChanges();

		return SortieRecord.CalculatedSortieCost.TotalAirBaseSupplyCost;
	}

	private SortieCostModel CalculateAirBaseSupplyCost()
	{
		SortieDetails ??= ToolService.GenerateSortieDetailViewModel(Db, SortieRecord);

		if (SortieDetails is null) return SortieCostModel.Zero;

		int aircraftLoss = GetAircraftLossFromAirBattles(SortieDetails.Nodes);

		if (SortieDetails.Nodes.Any(n => n.AirBaseRaid is not null))
		{
			aircraftLoss += GetAircraftLossFromRaid(SortieDetails);
		}

		return AirBaseResupplyCost(aircraftLoss);
	}

	/// <summary>
	/// Number of aircraft lost due to the base getting damaged.
	/// </summary>
	private static int GetAircraftLossFromRaid(SortieDetailViewModel sortieDetails)
	{
		IEnumerable<int> aircraftBeforeSortie = sortieDetails.FleetsBeforeSortie.AirBases
			.Zip(sortieDetails.StrikePoints, (ab, sp) => (AirBase: ab, StrikePoints: sp))
			.Where(t => t.StrikePoints is not null)
			.Select(t => t.AirBase)
			.Select(a => a.Squadrons.Values.Sum(s => s.AircraftCurrent));

		IEnumerable<int>? aircraftInBattle = sortieDetails.Nodes
			.SelectMany(n => n.AllPhases)
			.OfType<PhaseBaseAirAttack>()
			.FirstOrDefault()
			?.Units
			.DistinctBy(u => u.AirBaseId)
			.Select(u => u.Stage1FCount);

		if (aircraftInBattle is null) return 0;

		int aircraftLostInRaid = aircraftBeforeSortie
			.Zip(aircraftInBattle, (s, b) => s - b)
			.Sum();

		// can't lose more than 4 aircraft per air base in a raid
		// a squadron can't go under 1 aircraft as a result of an air raid
		int maxAircraftLossInRaid = sortieDetails.FleetsBeforeSortie.AirBases
			.Select(a => a.Squadrons.Values)
			.Select(c => c.Sum(s => Math.Max(0, s.AircraftCurrent - 1)))
			.Sum(c => Math.Min(4, c));

		if (aircraftLostInRaid > maxAircraftLossInRaid)
		{
			// todo: log or something, more tests would be good
			return 0;
		}

		return aircraftLostInRaid;
	}

	private static int GetAircraftLossFromAirBattles(IEnumerable<SortieNode> nodes) => nodes
		.SelectMany(n => n.AllPhases)
		.Sum(p => p switch
		{
			PhaseBaseAirRaid r => r.Stage1FLostcount + r.Stage2FLostcount,
			PhaseBaseAirAttack r => r.Units
				.Select((u, i) => (i % 2) switch
				{
					1 => u.Stage1FLostcount + u.Stage2FLostcount,
					_ => 0,
				})
				.Sum(),

			_ => 0,
		});

	private SortieCostModel? TryGetAirBaseSupplyCostFromDatabase(List<IBaseAirCorpsData> airBases)
	{
		if (TryGetAirBaseState(Db, Time) is not List<ApiAirBase> airBaseState)
		{
			return null;
		}

		if (TryGetCostFromState(airBases, airBaseState) is SortieCostModel sortieCost)
		{
			return sortieCost;
		}

		return null;
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

		if (airBases.Count is 0) return SortieCostModel.Zero;

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

		return AirBaseResupplyCost(aircraftLost);
	}

	private static SortieCostModel AirBaseResupplyCost(int aircraftLost) => new()
	{
		Fuel = 3 * aircraftLost,
		Bauxite = 5 * aircraftLost,
	};

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
				}
			}
		}

		return true;
	}
}
