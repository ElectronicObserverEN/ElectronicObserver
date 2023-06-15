using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public class BattleFleets
{
	public IFleetData Fleet { get; }
	public IFleetData? EscortFleet { get; }
	public List<IFleetData?>? Fleets { get; }
	public List<IBaseAirCorpsData> AirBases { get; }
	public IFleetData? FriendFleet { get; set; }
	public IFleetData? EnemyFleet { get; set; }
	public IFleetData? EnemyEscortFleet { get; set; }

	public BattleFleets(IFleetData fleet, IFleetData? escortFleet = null, List<IFleetData?>? fleets = null,
		List<IBaseAirCorpsData>? airBases = null)
	{
		Fleet = fleet;
		EscortFleet = escortFleet;
		Fleets = fleets;
		AirBases = airBases ?? new();
	}

	public BattleFleets Clone() => new(CloneFleet(Fleet), CloneFleet(EscortFleet), Fleets, AirBases.Select(CloneAirBase).ToList())
	{
		EnemyFleet = CloneFleet(EnemyFleet),
		EnemyEscortFleet = CloneFleet(EnemyEscortFleet),
	};

	[return: NotNullIfNotNull(nameof(fleet))]
	private static IFleetData? CloneFleet(IFleetData? fleet) => fleet switch
	{
		null => null,
		_ => fleet.DeepClone(),
	};

	[return: NotNullIfNotNull(nameof(ab))]
	private static IBaseAirCorpsData? CloneAirBase(IBaseAirCorpsData? ab) => ab switch
	{
		null => null,
		_ => ab.DeepClone(),
	};

	public IShipData? GetShip(BattleIndex index) => index.FleetFlag switch
	{
		FleetFlag.Player => (index.Index < Fleet.MembersInstance.Count) switch
		{
			true => Fleet.MembersInstance[index.Index],
			_ => EscortFleet?.MembersInstance[index.Index - Fleet.MembersInstance.Count],
		},

		_ => (index.Index < EnemyFleet?.MembersInstance.Count) switch
		{
			true => EnemyFleet?.MembersInstance[index.Index],
			_ => EnemyEscortFleet?.MembersInstance[index.Index - EnemyFleet?.MembersInstance.Count ?? 6],
		},
	};

	public IShipData? GetFriendShip(BattleIndex index) => index.FleetFlag switch
	{
		FleetFlag.Player when FriendFleet is not null => FriendFleet.MembersInstance[index.Index],

		_ => (index.Index < EnemyFleet?.MembersInstance.Count) switch
		{
			true => EnemyFleet?.MembersInstance[index.Index],
			_ => EnemyEscortFleet?.MembersInstance[index.Index - EnemyFleet?.MembersInstance.Count ?? 6],
		},
	};

	public IBaseAirCorpsData? GetAirBase(BattleIndex index) => index.FleetFlag switch
	{
		FleetFlag.Player => AirBases[index.Index],
	};

	/// <summary>
	/// Main problem here is if there's a dupe ship in combined fleet.
	/// Right now it matches via equipment too, but there's a chance both dupes have the same equip.
	/// </summary>
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
	private IShipData? GetShip(ApiShip shipData)
	{
		IEnumerable<IShipData?> ships = Fleet.MembersInstance;

		if (EscortFleet is not null)
		{
			ships = ships.Concat(EscortFleet.MembersInstance);
		}

		ships = ships.Where(s => s?.MasterShip.ShipId == shipData.ApiShipId);

		if (ships.Count() is 1)
		{
			return ships.First();
		}

		ships = ships.Where(s => s!.ExpansionSlot == shipData.ApiSlotEx);

		if (ships.Count() is 1)
		{
			return ships.First();
		}

		ships = ships.Where(s => s!.Slot.SequenceEqual(shipData.ApiSlot));

		if (ships.Count() is 1)
		{
			return ships.First();
		}

		// might need to do some other checks?
		return ships.First();
	}

	public void UpdateState(ApiGetMemberShipDeckResponse deck)
	{
		foreach (ApiShip shipData in deck.ApiShipData)
		{
			if (GetShip(shipData) is not ShipDataMock ship) continue;

			ship.Aircraft = shipData.ApiOnslot;
		}
	}
}
