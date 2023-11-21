using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.FitBonus;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public static class GameDataExtensions
{
	private static IKCDatabase KcDatabase => Ioc.Default.GetRequiredService<IKCDatabase>();
	private static List<FitBonusPerEquipment> FitBonusList => KCDatabase.Instance.Translation.FitBonus.FitBonusList;

	[return: NotNullIfNotNull(nameof(fleetData))]
	public static List<IFleetData?>? MakeFleets(this SortieFleetData? fleetData) => fleetData switch
	{
		{ } => fleetData.Fleets.Select((f, i) => MakeFleet(f, i switch
		{
			0 or 1 => fleetData.CombinedFlag,
			_ => 0,
		})).ToList(),
		_ => null,
	};

	[return: NotNullIfNotNull(nameof(fleet))]
	public static IFleetData? MakeFleet(this SortieFleet? fleet, int combinedFlag) => fleet switch
	{
		{ } => new FleetDataMock
		{
			Name = fleet.Name,
			FleetType = (FleetType)combinedFlag,
			MembersInstance = new ReadOnlyCollection<IShipData?>(fleet.Ships.Select(MakeShip).ToList()),
		},
		_ => null,
	};

	[return: NotNullIfNotNull(nameof(shipData))]
	private static IShipData? MakeShip(SortieShip? shipData)
	{
		if (shipData is null) return null;

		ShipDataMock ship = new(KcDatabase.MasterShips[(int)shipData.Id])
		{
			MasterID = shipData.DropId ?? 0,
			Level = shipData.Level,
			Condition = shipData.Condition,
			IsExpansionSlotAvailable = shipData.ExpansionSlot is not null,
			SlotInstance = shipData.EquipmentSlots.Select(s => MakeEquipment(s.Equipment)).ToList(),
			ExpansionSlotInstance = MakeEquipment(shipData.ExpansionSlot?.Equipment),
			Fuel = shipData.Fuel,
			Ammo = shipData.Ammo,
			Range = shipData.Range,
			Speed = shipData.Speed,
			LuckModernized = shipData.Kyouka.Skip(4).FirstOrDefault(),
			HPMaxModernized = shipData.Kyouka.Skip(5).FirstOrDefault(),
			ASWModernized = shipData.Kyouka.Skip(6).FirstOrDefault(),
		};

		if (shipData.Aircraft is not null)
		{
			ship.Aircraft = shipData.Aircraft;
		}

		return ApplyFitBonus(ship, shipData);
	}

	[return: NotNullIfNotNull(nameof(ship))]
	private static ShipDataMock? ApplyFitBonus(ShipDataMock? ship, SortieShip shipData)
	{
		if (ship is null) return ship;

		if (shipData.Firepower is null)
		{
			// old data, need to calculate fits
			FitBonusValue fit = ship.GetTheoricalFitBonus(FitBonusList);

			ship.FirepowerFit = fit.Firepower;
			ship.TorpedoFit = fit.Torpedo;
			ship.AaFit = fit.AntiAir;
			ship.ArmorFit = fit.Armor;
			ship.EvasionFit = fit.Evasion;
			ship.AswFit = fit.ASW;
			ship.LosFit = fit.LOS;
		}
		else
		{
			// none of them should be null for new data
			ship.FirepowerFit = shipData.Firepower.Value - ship.FirepowerTotal;
			ship.TorpedoFit = shipData.Torpedo!.Value - ship.TorpedoTotal;
			ship.AaFit = shipData.Aa!.Value - ship.AATotal;
			ship.ArmorFit = shipData.Armor!.Value - ship.ArmorTotal;
			ship.EvasionFit = shipData.Evasion!.Value - ship.EvasionTotal;
			ship.AswFit = shipData.Asw!.Value - ship.ASWTotal;
			ship.LosFit = shipData.Search!.Value - ship.LOSTotal;
		}

		return ship;
	}

	[return: NotNullIfNotNull(nameof(equipment))]
	private static IEquipmentData? MakeEquipment(SortieEquipment? equipment) => equipment switch
	{
		{ } => new EquipmentDataMock(KcDatabase.MasterEquipments[(int)equipment.Id])
		{
			Level = equipment.Level,
			AircraftLevel = equipment.AircraftLevel,
		},
		_ => null,
	};

	[return: NotNullIfNotNull(nameof(airBase))]
	public static IBaseAirCorpsData? MakeAirBase(this SortieAirBase? airBase) => airBase switch
	{
		{ } => new BaseAirCorpsDataMock
		{
			Name = airBase.Name,
			ActionKind = airBase.ActionKind,
			Distance = airBase.BaseDistance + airBase.BonusDistance,
			Squadrons = new Dictionary<int, IBaseAirCorpsSquadron>
			{
				{0, MakeAirBaseSquadron(airBase.Squadrons.Skip(0).FirstOrDefault())},
				{1, MakeAirBaseSquadron(airBase.Squadrons.Skip(1).FirstOrDefault())},
				{2, MakeAirBaseSquadron(airBase.Squadrons.Skip(2).FirstOrDefault())},
				{3, MakeAirBaseSquadron(airBase.Squadrons.Skip(3).FirstOrDefault())},
			},
		},
		_ => null,
	};

	private static IBaseAirCorpsSquadron MakeAirBaseSquadron(SortieAirBaseSquadron? squadron)
	{
		BaseAirCorpsSquadronMock abSlot = new()
		{
			Condition = squadron?.Condition ?? 0,
			EquipmentInstance = MakeEquipment(squadron?.EquipmentSlot.Equipment),
		};

		abSlot.AircraftMax = abSlot.EquipmentInstance?.MasterEquipment.AirBaseAircraftCount() ?? 0;
		abSlot.AircraftCurrent = squadron?.AircraftCurrent ?? abSlot.EquipmentInstance?.MasterEquipment.AirBaseAircraftCount() ?? 0;

		return abSlot;
	}

	public static IFleetData DeepClone(this IFleetData fleet) => new FleetDataMock
	{
		FleetID = fleet.FleetID,
		Name = fleet.Name,
		FleetType = fleet.FleetType,
		ExpeditionState = fleet.ExpeditionState,
		ExpeditionDestination = fleet.ExpeditionDestination,
		ExpeditionTime = fleet.ExpeditionTime,
		MembersInstance = new(fleet.MembersInstance.Select(DeepClone).ToList()),
		EscapedShipList = new(fleet.EscapedShipList),
		IsInSortie = fleet.IsInSortie,
		IsInPractice = fleet.IsInPractice,
		ID = fleet.ID,
		SupportType = fleet.SupportType,
		IsFlagshipRepairShip = fleet.IsFlagshipRepairShip,
		CanAnchorageRepair = fleet.CanAnchorageRepair,
		ConditionTime = fleet.ConditionTime,
		RequestData = fleet.RequestData,
		RawData = fleet.RawData,
		IsAvailable = fleet.IsAvailable,
	};

	[return: NotNullIfNotNull(nameof(ship))]
	private static IShipData? DeepClone(this IShipData? ship) => ship switch
	{
		null => null,
		_ => new ShipDataMock(ship.MasterShip)
		{
			// todo: rest of the stats and equipment deep clone
			MasterID = ship.MasterID,
			Level = ship.Level,
			Condition = ship.Condition,
			IsExpansionSlotAvailable = ship.IsExpansionSlotAvailable,
			SlotInstance = ship.SlotInstance,
			ExpansionSlotInstance = ship.ExpansionSlotInstance,
			Aircraft = ship.Aircraft,
			HPCurrent = ship.HPCurrent,
			Fuel = ship.Fuel,
			Ammo = ship.Ammo,
			Range = ship.Range,
			Speed = ship.Speed,
			LuckModernized = ship.LuckModernized,
			HPMaxModernized = ship.HPMaxModernized,
			ASWModernized = ship.ASWModernized,
			FirepowerFit = ship.FirepowerTotal - ship.FirepowerBase - ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Firepower ?? 0),
			TorpedoFit = ship.TorpedoTotal - ship.TorpedoBase - ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Torpedo ?? 0),
			AaFit = ship.AATotal - ship.AABase - ship.AllSlotInstance.Sum(e => e?.MasterEquipment.AA ?? 0),
			ArmorFit = ship.ArmorTotal - ship.ArmorBase - ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Armor ?? 0),
			EvasionFit = ship.EvasionTotal - ship.EvasionBase - ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Evasion ?? 0),
			AswFit = ship.ASWTotal - ship.ASWBase - ship.AllSlotInstance.Sum(e => e?.MasterEquipment.ASW ?? 0),
			LosFit = ship.LOSTotal - ship.LOSBase - ship.AllSlotInstance.Sum(e => e?.MasterEquipment.LOS ?? 0),
		},
	};

	public static IBaseAirCorpsData DeepClone(this IBaseAirCorpsData ab) => new BaseAirCorpsDataMock
	{
		MapAreaID = ab.MapAreaID,
		AirCorpsID = ab.AirCorpsID,
		Name = ab.Name,
		Distance = ab.Distance,
		Bonus_Distance = ab.Bonus_Distance,
		Base_Distance = ab.Base_Distance,
		ActionKind = ab.ActionKind,
		StrikePoints = ab.StrikePoints,
		Squadrons = ab.Squadrons.ToDictionary(kvp => kvp.Key, kvp => DeepClone(kvp.Value)),
		ID = ab.ID,
		IsAvailable = ab.IsAvailable,
		HPCurrent = ab.HPCurrent,
		HPMax = ab.HPMax,
	};

	private static IBaseAirCorpsSquadron DeepClone(this IBaseAirCorpsSquadron sq) => new BaseAirCorpsSquadronMock
	{
		SquadronID = sq.SquadronID,
		State = sq.State,
		EquipmentMasterID = sq.EquipmentMasterID,
		EquipmentInstance = sq.EquipmentInstance,
		EquipmentInstanceMaster = sq.EquipmentInstanceMaster,
		AircraftCurrent = sq.AircraftCurrent,
		AircraftMax = sq.AircraftMax,
		Condition = sq.Condition,
		RelocatedTime = sq.RelocatedTime,
		ID = sq.ID,
		IsAvailable = sq.IsAvailable,
	};
}
