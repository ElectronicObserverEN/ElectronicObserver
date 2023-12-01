using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.AirControlSimulator;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.AirControlSimulator;
using ElectronicObserverTypes.Serialization.DeckBuilder;
using ElectronicObserverTypes.Serialization.EventLockPlanner;
using ElectronicObserverTypes.Serialization.FleetAnalysis;

namespace ElectronicObserver.Services;

public class DataSerializationService
{
	private static JsonSerializerOptions JsonSerializerOptions => new()
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
	};

	public string FleetAnalysisShips(bool allShips)
	{
		return FleetAnalysisShips(KCDatabase.Instance.Ships.Values
			.Where(s => allShips || s.IsLocked));
	}

	public string FleetAnalysisEquipment(bool allEquipment)
	{
		return FleetAnalysisEquipment(KCDatabase.Instance.Equipments.Values
			.Where(eq => allEquipment || eq.IsLocked));
	}

	private static FleetData? FleetOrDefault(bool include, int index) => include switch
	{
		false => null,
		_ => KCDatabase.Instance.Fleet.Fleets.Values.Skip(index).FirstOrDefault(),
	};

	public string AirControlSimulatorLink(AirControlSimulatorViewModel airControlSimulator,
		SortieDetailViewModel? sortieDetail)
	{
		List<BaseAirCorpsData> bases = KCDatabase.Instance.BaseAirCorps.Values
			.Where(b => b.MapAreaID == airControlSimulator.AirBaseArea?.AreaId)
			.ToList();

		string airControlSimulatorData = AirControlSimulator
		(
			KCDatabase.Instance.Admiral.Level,
			FleetOrDefault(airControlSimulator.Fleet1, 0),
			FleetOrDefault(airControlSimulator.Fleet2, 1),
			FleetOrDefault(airControlSimulator.Fleet3, 2),
			FleetOrDefault(airControlSimulator.Fleet4, 3),
			bases.Skip(0).FirstOrDefault(),
			bases.Skip(1).FirstOrDefault(),
			bases.Skip(2).FirstOrDefault(),
			sortieDetail,
			airControlSimulator.ShipData switch
			{
				true => KCDatabase.Instance.Ships.Values
					.Where(s => airControlSimulator.IncludeUnlockedShips || s.IsLocked),
				_ => null,
			},
			airControlSimulator.EquipmentData switch
			{
				true => KCDatabase.Instance.Equipments.Values
					.Where(e => airControlSimulator.IncludeUnlockedEquipment || e!.IsLocked)
					.Cast<EquipmentData>(),
				_ => null,
			},
			airControlSimulator.MaxAircraftLevelFleet,
			airControlSimulator.MaxAircraftLevelAirBase
		);

		return @$"https://noro6.github.io/kc-web#import:{airControlSimulatorData}";
	}

	public string AirControlSimulator
	(
		int hqLevel,
		IFleetData? fleet1 = null,
		IFleetData? fleet2 = null,
		IFleetData? fleet3 = null,
		IFleetData? fleet4 = null,
		IBaseAirCorpsData? airBase1 = null,
		IBaseAirCorpsData? airBase2 = null,
		IBaseAirCorpsData? airBase3 = null,
		SortieDetailViewModel? sortieDetails = null,
		IEnumerable<IShipData>? ships = null,
		IEnumerable<IEquipmentData>? equipment = null,
		bool maxAircraftLevelFleet = false,
		bool maxAircraftLevelAirBase = false) => JsonSerializer.Serialize
	(
		MakeAirControlSimulatorData
		(
			MakeDeckBuilderData(hqLevel, fleet1, fleet2, fleet3, fleet4, airBase1, airBase2, airBase3, sortieDetails, maxAircraftLevelFleet, maxAircraftLevelAirBase),
			ships?.Select(MakeFleetAnalysisShip),
			equipment?.Select(MakeFleetAnalysisEquipment)
		),
		JsonSerializerOptions
	);

	public string OperationRoomLink(AirControlSimulatorViewModel airControlSimulator,
		SortieDetailViewModel? sortieDetail = null)
	{
		List<BaseAirCorpsData> bases = KCDatabase.Instance.BaseAirCorps.Values
			.Where(b => b.MapAreaID == airControlSimulator.AirBaseArea?.AreaId)
			.ToList();

		string operationRoomData = DeckBuilder
		(
			KCDatabase.Instance.Admiral.Level,
			FleetOrDefault(airControlSimulator.Fleet1, 0),
			FleetOrDefault(airControlSimulator.Fleet2, 1),
			FleetOrDefault(airControlSimulator.Fleet3, 2),
			FleetOrDefault(airControlSimulator.Fleet4, 3),
			bases.Skip(0).FirstOrDefault(),
			bases.Skip(1).FirstOrDefault(),
			bases.Skip(2).FirstOrDefault(),
			sortieDetail,
			airControlSimulator.MaxAircraftLevelFleet,
			airControlSimulator.MaxAircraftLevelAirBase
		);

		return @$"https://jervis.vercel.app?predeck={Uri.EscapeDataString(operationRoomData)}";
	}

	public string DeckBuilder
	(
		int hqLevel,
		IFleetData? fleet1 = null,
		IFleetData? fleet2 = null,
		IFleetData? fleet3 = null,
		IFleetData? fleet4 = null,
		IBaseAirCorpsData? airBase1 = null,
		IBaseAirCorpsData? airBase2 = null,
		IBaseAirCorpsData? airBase3 = null,
		SortieDetailViewModel? sortieDetails = null,
		bool maxAircraftLevelFleet = false,
		bool maxAircraftLevelAirBase = false
	) => JsonSerializer.Serialize
	(
		MakeDeckBuilderData(hqLevel, fleet1, fleet2, fleet3, fleet4, airBase1, airBase2, airBase3, sortieDetails, maxAircraftLevelFleet, maxAircraftLevelAirBase),
		JsonSerializerOptions
	);

	public static string FleetAnalysisShips(IEnumerable<IShipData> ships) =>
		JsonSerializer.Serialize(MakeFleetAnalysisShips(ships), JsonSerializerOptions);

	public static string FleetAnalysisEquipment(IEnumerable<IEquipmentData> equipment) =>
		JsonSerializer.Serialize(MakeFleetAnalysisEquipment(equipment), JsonSerializerOptions);

	private static AirControlSimulatorData MakeAirControlSimulatorData
	(
		DeckBuilderData? fleet = null,
		IEnumerable<FleetAnalysisShip>? ships = null,
		IEnumerable<FleetAnalysisEquipment>? equipment = null
	) => new()
	{
		Fleet = fleet,
		Ships = ships,
		Equipment = equipment
	};

	public DeckBuilderData MakeDeckBuilderData
	(
		int hqLevel,
		IFleetData? fleet1 = null,
		IFleetData? fleet2 = null,
		IFleetData? fleet3 = null,
		IFleetData? fleet4 = null,
		IBaseAirCorpsData? airBase1 = null,
		IBaseAirCorpsData? airBase2 = null,
		IBaseAirCorpsData? airBase3 = null,
		SortieDetailViewModel? sortieDetails = null,
		bool maxAircraftLevelFleet = false,
		bool maxAircraftLevelAirBase = false
	) => new()
	{
		HqLevel = hqLevel,
		Fleet1 = MakeDeckBuilderFleet(fleet1, maxAircraftLevelFleet),
		Fleet2 = MakeDeckBuilderFleet(fleet2, maxAircraftLevelFleet),
		Fleet3 = MakeDeckBuilderFleet(fleet3, maxAircraftLevelFleet),
		Fleet4 = MakeDeckBuilderFleet(fleet4, maxAircraftLevelFleet),
		AirBase1 = MakeDeckBuilderAirBase(airBase1, sortieDetails?.StrikePoints.Skip(0).FirstOrDefault(), maxAircraftLevelAirBase),
		AirBase2 = MakeDeckBuilderAirBase(airBase2, sortieDetails?.StrikePoints.Skip(1).FirstOrDefault(), maxAircraftLevelAirBase),
		AirBase3 = MakeDeckBuilderAirBase(airBase3, sortieDetails?.StrikePoints.Skip(2).FirstOrDefault(), maxAircraftLevelAirBase),
		Sortie = MakeDeckBuilderSortie(sortieDetails),
	};

	private static DeckBuilderFleet? MakeDeckBuilderFleet
	(
		IFleetData? fleet,
		bool maxAircraftLevel = false
	) => fleet switch
	{
		{ } f => new()
		{
			Name = fleet.Name,
			Type = fleet.FleetType,
			Ship1 = MakeDeckBuilderShip(f.MembersInstance.Skip(0).FirstOrDefault(), maxAircraftLevel),
			Ship2 = MakeDeckBuilderShip(f.MembersInstance.Skip(1).FirstOrDefault(), maxAircraftLevel),
			Ship3 = MakeDeckBuilderShip(f.MembersInstance.Skip(2).FirstOrDefault(), maxAircraftLevel),
			Ship4 = MakeDeckBuilderShip(f.MembersInstance.Skip(3).FirstOrDefault(), maxAircraftLevel),
			Ship5 = MakeDeckBuilderShip(f.MembersInstance.Skip(4).FirstOrDefault(), maxAircraftLevel),
			Ship6 = MakeDeckBuilderShip(f.MembersInstance.Skip(5).FirstOrDefault(), maxAircraftLevel),
			Ship7 = MakeDeckBuilderShip(f.MembersInstance.Skip(6).FirstOrDefault(), maxAircraftLevel),
		},

		_ => null
	};

	private static DeckBuilderShip? MakeDeckBuilderShip
	(
		IShipData? ship,
		bool maxAircraftLevel = false
	) => ship switch
	{
		{ } s => new()
		{
			Id = s.MasterShip.ShipId,
			Level = s.Level,
			IsExpansionSlotAvailable = s.IsExpansionSlotAvailable,
			Equipment = new()
			{
				Equipment1 = MakeDeckBuilderEquipment(s.AllSlotInstance.Skip(0).FirstOrDefault(), maxAircraftLevel),
				Equipment2 = MakeDeckBuilderEquipment(s.AllSlotInstance.Skip(1).FirstOrDefault(), maxAircraftLevel),
				Equipment3 = MakeDeckBuilderEquipment(s.AllSlotInstance.Skip(2).FirstOrDefault(), maxAircraftLevel),
				Equipment4 = MakeDeckBuilderEquipment(s.AllSlotInstance.Skip(3).FirstOrDefault(), maxAircraftLevel),
				Equipment5 = MakeDeckBuilderEquipment(s.AllSlotInstance.Skip(4).FirstOrDefault(), maxAircraftLevel),
				EquipmentExpansion = MakeDeckBuilderEquipment(s.ExpansionSlotInstance, maxAircraftLevel),
			},
			Hp = s.HPMax,
			Firepower = s.FirepowerTotal,
			Torpedo = s.TorpedoTotal,
			AntiAir = s.AATotal,
			Armor = s.ArmorTotal,
			AntiSubmarine = s.ASWTotal,
			Evasion = s.EvasionTotal,
			Los = s.LOSTotal,
			Luck = s.LuckTotal,
			Speed = s.Speed,
			Range = s.Range,
		},

		_ => null
	};

	private static DeckBuilderEquipment? MakeDeckBuilderEquipment
	(
		IEquipmentData? equipment,
		bool maxAircraftLevel
	) => equipment switch
	{
		{ } eq => new()
		{
			Id = eq.MasterEquipment.EquipmentId,
			Level = eq.Level,
			AircraftLevel = GetAircraftLevel(eq, maxAircraftLevel),
		},

		_ => null
	};

	private static int? GetAircraftLevel(IEquipmentData equipment, bool maxAircraftLevel) =>
		(equipment, maxAircraftLevel) switch
		{
			({ MasterEquipment.IsAircraft: true }, true) => 7,
			({ MasterEquipment.IsAircraft: true }, false) => equipment.AircraftLevel,
			_ => null
		};

	private static DeckBuilderAirBase? MakeDeckBuilderAirBase
	(
		IBaseAirCorpsData? airBase,
		List<int>? strikePoints,
		bool maxAircraftLevel
	) => airBase switch
	{
		{ } ab => new()
		{
			Name = airBase.Name,
			Equipment = new()
			{
				Equipment1 = MakeDeckBuilderEquipment(ab.Squadrons.Values.Skip(0).FirstOrDefault()?.EquipmentInstance, maxAircraftLevel),
				Equipment2 = MakeDeckBuilderEquipment(ab.Squadrons.Values.Skip(1).FirstOrDefault()?.EquipmentInstance, maxAircraftLevel),
				Equipment3 = MakeDeckBuilderEquipment(ab.Squadrons.Values.Skip(2).FirstOrDefault()?.EquipmentInstance, maxAircraftLevel),
				Equipment4 = MakeDeckBuilderEquipment(ab.Squadrons.Values.Skip(3).FirstOrDefault()?.EquipmentInstance, maxAircraftLevel),
			},
			Mode = airBase.ActionKind,
			Distance = airBase.Distance,
			StrikePoints = strikePoints,
		},

		_ => null,
	};

	private static DeckBuilderSortieData? MakeDeckBuilderSortie(SortieDetailViewModel? sortieDetails) => sortieDetails switch
	{
		null => null,
		_ => new()
		{
			MapAreaId = sortieDetails.World,
			MapInfoId = sortieDetails.Map,
			Cells = sortieDetails.Nodes
				.OfType<BattleNode>()
				.Select(MakeDeckBuilderCell)
				.ToList(),
		},
	};

	private static DeckBuilderCell MakeDeckBuilderCell(BattleNode node) => new DeckBuilderCell
	{
		CellId = node.Cell,
		PlayerFormation = node.FirstBattle.Phases
			.OfType<PhaseSearching>()
			.FirstOrDefault()
			?.PlayerFormationType
			?? FormationType.LineAhead,
		EnemyFormation = node.FirstBattle.Phases
			.OfType<PhaseSearching>()
			.FirstOrDefault()
			?.EnemyFormationType
			?? FormationType.LineAhead,
		Fleet1 = node.FirstBattle.FleetsBeforeBattle.EnemyFleet switch
		{
			{ } fleet => new()
			{
				Name = fleet.Name,
				Ships = fleet.MembersInstance
					.Where(s => s is not null)
					.Cast<IShipData>()
					.Select(MakeDeckBuilderEnemyShip)
					.ToList(),
			},
			// enemy fleet should never be null
			_ => new(),
		},
		Fleet2 = node.FirstBattle.FleetsBeforeBattle.EnemyEscortFleet switch
		{
			{ } fleet => new()
			{
				Name = fleet.Name,
				Ships = fleet.MembersInstance
					.Where(s => s is not null)
					.Cast<IShipData>()
					.Select(MakeDeckBuilderEnemyShip)
					.ToList(),
			},
			_ => null,
		},
	};

	private static DeckBuilderEnemyShip MakeDeckBuilderEnemyShip(IShipData ship) => new()
	{
		Id = ship.MasterShip.ShipId,
		Equipment = ship.AllSlotInstance
			.Where(e => e is not null)
			.Cast<IEquipmentData>()
			.Select(MakeDeckBuilderEnemyEquipment)
			.ToList(),
	};

	private static DeckBuilderEnemyEquipment MakeDeckBuilderEnemyEquipment(IEquipmentData equip) => new()
	{
		Id = equip.EquipmentId,
	};

	private static IEnumerable<FleetAnalysisShip> MakeFleetAnalysisShips(IEnumerable<IShipData> ships)
		=> ships.Select(MakeFleetAnalysisShip);

	private static FleetAnalysisShip MakeFleetAnalysisShip(IShipData ship) => new()
	{
		DropId = ship.MasterID,
		ShipId = ship.MasterShip.ShipId,
		Level = ship.Level,
		Modernization = new List<int>
		{
			ship.FirepowerModernized,
			ship.TorpedoModernized,
			ship.AAModernized,
			ship.ArmorModernized,
			ship.LuckModernized,
			ship.HPMaxModernized,
			ship.ASWModernized,
		},
		Experience = new List<double>
		{
			ship.ExpTotal,
			ship.ExpNext,
			ship.ExpNextPercentage
		},
		ExpansionSlot = ship.ExpansionSlot,
		SallyArea = ship.SallyArea,
	};

	private static IEnumerable<FleetAnalysisEquipment> MakeFleetAnalysisEquipment(IEnumerable<IEquipmentData> equipment)
		=> equipment.Select(MakeFleetAnalysisEquipment);

	private static FleetAnalysisEquipment MakeFleetAnalysisEquipment(IEquipmentData equipment) => new()
	{
		Id = equipment.MasterEquipment.EquipmentId,
		Level = equipment.Level,
	};

	public string EventLockPlanner(EventLockPlannerViewModel viewModel)
	{
		EventLockPlannerData data = new()
		{
			Locks = viewModel.LockGroups.Select(g => new EventLockPlannerLock
			{
				Id = g.Id,
				A = g.Color.A,
				R = g.Color.R,
				G = g.Color.G,
				B = g.Color.B,
				Name = g.Name,
			}).ToList(),
			Phases = viewModel.EventPhases.Select(p => new EventLockPlannerPhase
			{
				LockGroups = p.PhaseLockGroups.Select(g => g.Id).ToList(),
				Name = p.Name,
			}).ToList()
		};

		return JsonSerializer.Serialize(data, JsonSerializerOptions);
	}
}
