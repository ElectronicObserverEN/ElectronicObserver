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

		DeckBuilderParameters parameters = new()
		{
			HqLevel = KCDatabase.Instance.Admiral.Level,
			Fleet1 = FleetOrDefault(airControlSimulator.Fleet1, 0),
			Fleet2 = FleetOrDefault(airControlSimulator.Fleet2, 1),
			Fleet3 = FleetOrDefault(airControlSimulator.Fleet3, 2),
			Fleet4 = FleetOrDefault(airControlSimulator.Fleet4, 3),
			AirBase1 = bases.Skip(0).FirstOrDefault(),
			AirBase2 = bases.Skip(1).FirstOrDefault(),
			AirBase3 = bases.Skip(2).FirstOrDefault(),
			SortieDetails = sortieDetail,
			MaxAircraftLevelFleet = airControlSimulator.MaxAircraftLevelFleet,
			MaxAircraftLevelAirBase = airControlSimulator.MaxAircraftLevelAirBase,
		};

		string airControlSimulatorData = AirControlSimulator
		(
			parameters,
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
			}
		);

		return @$"https://noro6.github.io/kc-web#import:{airControlSimulatorData}";
	}

	public static string AirControlSimulator(DeckBuilderParameters parameters,
		IEnumerable<IShipData>? ships = null, IEnumerable<IEquipmentData>? equipment = null) =>
		JsonSerializer.Serialize
		(
			MakeAirControlSimulatorData
			(
				MakeDeckBuilderData(parameters),
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

		string operationRoomData = DeckBuilder(new()
		{
			HqLevel = KCDatabase.Instance.Admiral.Level,
			Fleet1 = FleetOrDefault(airControlSimulator.Fleet1, 0),
			Fleet2 = FleetOrDefault(airControlSimulator.Fleet2, 1),
			Fleet3 = FleetOrDefault(airControlSimulator.Fleet3, 2),
			Fleet4 = FleetOrDefault(airControlSimulator.Fleet4, 3),
			AirBase1 = bases.Skip(0).FirstOrDefault(),
			AirBase2 = bases.Skip(1).FirstOrDefault(),
			AirBase3 = bases.Skip(2).FirstOrDefault(),
			SortieDetails = sortieDetail,
			MaxAircraftLevelFleet = airControlSimulator.MaxAircraftLevelFleet,
			MaxAircraftLevelAirBase = airControlSimulator.MaxAircraftLevelAirBase
		});

		return @$"https://jervis.vercel.app?predeck={Uri.EscapeDataString(operationRoomData)}";
	}

	public string DeckBuilder(DeckBuilderParameters parameters) => JsonSerializer.Serialize
	(
		MakeDeckBuilderData(parameters),
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

	public static DeckBuilderData MakeDeckBuilderData(DeckBuilderParameters parameters) => new()
	{
		HqLevel = parameters.HqLevel,
		Fleet1 = MakeDeckBuilderFleet(parameters.Fleet1, parameters.MaxAircraftLevelFleet),
		Fleet2 = MakeDeckBuilderFleet(parameters.Fleet2, parameters.MaxAircraftLevelFleet),
		Fleet3 = MakeDeckBuilderFleet(parameters.Fleet3, parameters.MaxAircraftLevelFleet),
		Fleet4 = MakeDeckBuilderFleet(parameters.Fleet4, parameters.MaxAircraftLevelFleet),
		AirBase1 = MakeDeckBuilderAirBase(parameters.AirBase1, parameters.SortieDetails?.StrikePoints.Skip(0).FirstOrDefault(), parameters.MaxAircraftLevelAirBase),
		AirBase2 = MakeDeckBuilderAirBase(parameters.AirBase2, parameters.SortieDetails?.StrikePoints.Skip(1).FirstOrDefault(), parameters.MaxAircraftLevelAirBase),
		AirBase3 = MakeDeckBuilderAirBase(parameters.AirBase3, parameters.SortieDetails?.StrikePoints.Skip(2).FirstOrDefault(), parameters.MaxAircraftLevelAirBase),
		Sortie = MakeDeckBuilderSortie(parameters.SortieDetails),
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

	private static DeckBuilderCell MakeDeckBuilderCell(BattleNode node) => new()
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
