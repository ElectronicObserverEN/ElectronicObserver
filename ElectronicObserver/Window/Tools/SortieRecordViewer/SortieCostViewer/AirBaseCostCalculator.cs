using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Mapinfo;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class AirBaseCostCalculator(ElectronicObserverContext db, SortieRecordViewModel sortie)
{
	private ElectronicObserverContext Db { get; } = db;

	private DateTime Time { get; } = sortie.SortieStart.ToUniversalTime();
	private List<IBaseAirCorpsData> AirBases { get; } = sortie.Model.FleetData.AirBases
		.Select(a => a.MakeAirBase())
		.ToList();

	public SortieCostModel AirBaseSortieCost(IBaseAirCorpsData airBase)
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

	public SortieCostModel AirBaseSupplyCost()
	{
		// todo: get plane shotdown from battle details
		return TryGetAirBaseSupplyCostFromDatabase() ??  new();
	}

	private SortieCostModel? TryGetAirBaseSupplyCostFromDatabase()
	{
		if (TryGetAirBaseState(Db, Time) is not List<ApiAirBase> airBaseState)
		{
			return null;
		}

		if (TryGetCostFromState(AirBases, airBaseState) is SortieCostModel sortieCost)
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
