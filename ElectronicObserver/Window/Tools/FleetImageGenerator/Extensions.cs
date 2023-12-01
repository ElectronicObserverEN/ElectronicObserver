using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.DeckBuilder;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

// todo: extension methods probably aren't the best solution for converting deckbuilder data to ship/equip data
// it would probably be better to make a data import service or something like that
// main reason for this not being a good idea is the Db property
public static class Extensions
{
	private static IKCDatabase Db { get; } = Ioc.Default.GetRequiredService<IKCDatabase>();

	public static List<IFleetData?> GetFleetList(this DeckBuilderData deckBuilderData) => new()
	{
		ToFleetData(deckBuilderData.Fleet1),
		ToFleetData(deckBuilderData.Fleet2),
		ToFleetData(deckBuilderData.Fleet3),
		ToFleetData(deckBuilderData.Fleet4),
	};

	private static IFleetData? ToFleetData(DeckBuilderFleet? deckBuilderFleet) => deckBuilderFleet switch
	{
		null => null,

		_ => new FleetDataMock
		{
			Name = deckBuilderFleet.Name ?? "",
			FleetType = deckBuilderFleet.Type,
			MembersInstance = new(new List<IShipData?>
			{
				ToShipData(deckBuilderFleet.Ship1),
				ToShipData(deckBuilderFleet.Ship2),
				ToShipData(deckBuilderFleet.Ship3),
				ToShipData(deckBuilderFleet.Ship4),
				ToShipData(deckBuilderFleet.Ship5),
				ToShipData(deckBuilderFleet.Ship6),
				ToShipData(deckBuilderFleet.Ship7),
			}),
		},
	};

	private static IShipData? ToShipData(DeckBuilderShip? deckBuilderShip)
	{
		if (deckBuilderShip == null)
		{
			return null;
		}

		ShipDataMock ship = new(Db.MasterShips[(int)deckBuilderShip.Id])
		{
			Level = deckBuilderShip.Level,
			IsExpansionSlotAvailable = deckBuilderShip.IsExpansionSlotAvailable,
			SlotInstance = new List<IEquipmentData?>
			{
				ToEquipmentData(deckBuilderShip.Equipment.Equipment1),
				ToEquipmentData(deckBuilderShip.Equipment.Equipment2),
				ToEquipmentData(deckBuilderShip.Equipment.Equipment3),
				ToEquipmentData(deckBuilderShip.Equipment.Equipment4),
				ToEquipmentData(deckBuilderShip.Equipment.Equipment5),
			},
			ExpansionSlotInstance = ToEquipmentData(deckBuilderShip.Equipment.EquipmentExpansion),
		};

		ship.HPMaxModernized = deckBuilderShip.Hp - ship.HPMax;

		IEnumerable<IEquipmentDataMaster> equip = ship.AllSlotInstance
			.Where(e => e is not null)
			.Select(e => e!.MasterEquipment);

		ship.FirepowerFit = deckBuilderShip.Firepower - ship.FirepowerBase - equip.Sum(e => e.Firepower);
		ship.TorpedoFit = deckBuilderShip.Torpedo - ship.TorpedoBase - equip.Sum(e => e.Torpedo);
		ship.AaFit = deckBuilderShip.AntiAir - ship.AABase - equip.Sum(e => e.AA);
		ship.ArmorFit = deckBuilderShip.Armor - ship.ArmorBase - equip.Sum(e => e.Armor);

		// it's not really possible to know how much ASW came from fits/modernization
		// while we can determine the current fits, these can get changed over time
		ship.ASWModernized = deckBuilderShip.AntiSubmarine - ship.ASWBase - equip.Sum(e => e.ASW);
		ship.EvasionFit = deckBuilderShip.Evasion - ship.EvasionBase - equip.Sum(e => e.Evasion);
		ship.LosFit = deckBuilderShip.Los - ship.LOSBase - equip.Sum(e => e.LOS);
		ship.LuckModernized = deckBuilderShip.Luck - ship.MasterShip.LuckMin - equip.Sum(e => e.Luck);

		ship.Speed = deckBuilderShip.Speed;
		ship.Range = deckBuilderShip.Range;

		return ship;
	}

	private static IEquipmentData? ToEquipmentData(DeckBuilderEquipment? deckBuilderEquipment) =>
		deckBuilderEquipment switch
		{
			null => null,

			_ => new EquipmentDataMock(Db.MasterEquipments[(int)deckBuilderEquipment.Id])
			{
				Level = deckBuilderEquipment.Level,
				AircraftLevel = deckBuilderEquipment.AircraftLevel ?? 0,
			},
		};

	public static List<IBaseAirCorpsData?> GetAirBaseList(this DeckBuilderData deckBuilderData) => new()
	{
		ToBaseAirCorpsData(deckBuilderData.AirBase1),
		ToBaseAirCorpsData(deckBuilderData.AirBase2),
		ToBaseAirCorpsData(deckBuilderData.AirBase3),
	};

	private static IBaseAirCorpsData? ToBaseAirCorpsData(DeckBuilderAirBase? deckBuilderAirBase) => deckBuilderAirBase switch
	{
		{ } => new BaseAirCorpsDataMock
		{
			Name = deckBuilderAirBase.Name ?? "",
			ActionKind = deckBuilderAirBase.Mode,
			Distance = deckBuilderAirBase.Distance,
			Squadrons = new Dictionary<int, IBaseAirCorpsSquadron>
			{
				{0, ToBaseAirCorpsSquadron(deckBuilderAirBase.Equipment.Equipment1)},
				{1, ToBaseAirCorpsSquadron(deckBuilderAirBase.Equipment.Equipment2)},
				{2, ToBaseAirCorpsSquadron(deckBuilderAirBase.Equipment.Equipment3)},
				{3, ToBaseAirCorpsSquadron(deckBuilderAirBase.Equipment.Equipment4)},
			},
		},

		_ => null,
	};

	private static IBaseAirCorpsSquadron ToBaseAirCorpsSquadron(DeckBuilderEquipment? deckBuilderEquipment)
	{
		BaseAirCorpsSquadronMock abSlot = new()
		{
			EquipmentInstance = ToEquipmentData(deckBuilderEquipment),
		};

		abSlot.AircraftMax = abSlot.EquipmentInstance?.MasterEquipment.AirBaseAircraftCount() ?? 0;
		abSlot.AircraftCurrent = abSlot.EquipmentInstance?.MasterEquipment.AirBaseAircraftCount() ?? 0;

		return abSlot;
	}
}
