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

	public static IFleetData? MakeFleet(this SortieFleet? fleet) => fleet switch
	{
		{ } => new FleetDataMock
		{
			Name = fleet.Name,
			MembersInstance = new ReadOnlyCollection<IShipData?>(fleet.Ships.Select(MakeShip).Select(ApplyFitBonus).ToList()),
		},
		_ => null,
	};

	private static ShipDataMock? MakeShip(SortieShip? ship) => ship switch
	{
		{ } => new ShipDataMock(KcDatabase.MasterShips[(int)ship.Id])
		{
			Level = ship.Level,
			IsExpansionSlotAvailable = ship.ExpansionSlot is not null,
			SlotInstance = ship.EquipmentSlots.Select(s => MakeEquipment(s.Equipment)).ToList(),
			ExpansionSlotInstance = MakeEquipment(ship.ExpansionSlot?.Equipment),
			Fuel = ship.Fuel,
			Ammo = ship.Ammo,
			Range = ship.Range,
			Speed = ship.Speed,
			LuckModernized = ship.Kyouka.Skip(4).FirstOrDefault(),
			HPMaxModernized = ship.Kyouka.Skip(5).FirstOrDefault(),
			ASWModernized = ship.Kyouka.Skip(6).FirstOrDefault(),
		},
		_ => null,
	};

	private static IShipData? ApplyFitBonus(ShipDataMock? ship)
	{
		if (ship is null) return ship;

		FitBonusValue fit = ship.GetFitBonus(FitBonusList);

		ship.FirepowerFit = fit.Firepower;
		ship.TorpedoFit = fit.Torpedo;
		ship.AaFit = fit.AntiAir;
		ship.ArmorFit = fit.Armor;
		ship.EvasionFit = fit.Evasion;
		ship.AswFit = fit.ASW;
		ship.LosFit = fit.LOS;

		return ship;
	}

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
			EquipmentInstance = MakeEquipment(squadron?.EquipmentSlot.Equipment),
		};

		abSlot.AircraftMax = abSlot.EquipmentInstance?.MasterEquipment.AirBaseAircraftCount() ?? 0;
		abSlot.AircraftCurrent = abSlot.EquipmentInstance?.MasterEquipment.AirBaseAircraftCount() ?? 0;

		return abSlot;
	}

	private static int AirBaseAircraftCount(IEquipmentDataMaster? equipment) => equipment?.CategoryType switch
	{
		null => 0,

		EquipmentTypes.CarrierBasedRecon => 4,
		EquipmentTypes.FlyingBoat => 4,
		EquipmentTypes.HeavyBomber => 9,

		_ => 18,
	};

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

	private static IShipData? DeepClone(this IShipData? ship) => ship switch
	{
		null => null,
		_ => new ShipDataMock(ship.MasterShip)
		{
			// todo: rest of the stats and equipment deep clone
			Level = ship.Level,
			IsExpansionSlotAvailable = ship.IsExpansionSlotAvailable,
			SlotInstance = ship.SlotInstance,
			ExpansionSlotInstance = ship.ExpansionSlotInstance,
			HPCurrent = ship.HPCurrent,
			Fuel = ship.Fuel,
			Ammo = ship.Ammo,
			Range = ship.Range,
			Speed = ship.Speed,
			LuckModernized = ship.LuckModernized,
			HPMaxModernized = ship.HPMaxModernized,
			ASWModernized = ship.ASWModernized,
			// FirepowerFit = ship.FirepowerFit,
			// TorpedoFit = ship.TorpedoFit,
			// AaFit = ship.AaFit,
			// ArmorFit = ship.ArmorFit,
			// EvasionFit = ship.EvasionFit,
			// AswFit = ship.AswFit,
			// LosFit = ship.LosFit,
		},
	};
}
