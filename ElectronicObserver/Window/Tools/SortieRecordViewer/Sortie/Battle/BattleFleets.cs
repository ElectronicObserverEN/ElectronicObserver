using System;
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

	private int CombinedFleetMainFleetShipCount => 6;

	public BattleFleets(IFleetData fleet, IFleetData? escortFleet = null, List<IFleetData?>? fleets = null,
		List<IBaseAirCorpsData>? airBases = null)
	{
		Fleet = fleet;
		EscortFleet = escortFleet;
		Fleets = fleets;
		AirBases = airBases ?? new();
	}

	public BattleFleets Clone() => new(CloneFleet(Fleet), CloneFleet(EscortFleet), Fleets, AirBases.Select(CloneAirBase).ToList()!)
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
		FleetFlag.Player => (index.Index < CombinedFleetMainFleetShipCount) switch
		{
			true => Fleet.MembersInstance[index.Index],
			_ => EscortFleet?.MembersInstance[index.Index - CombinedFleetMainFleetShipCount],
		},

		_ => (index.Index < CombinedFleetMainFleetShipCount) switch
		{
			true => EnemyFleet?.MembersInstance[index.Index],
			_ => EnemyEscortFleet?.MembersInstance[index.Index - CombinedFleetMainFleetShipCount],
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
		_ => null,
	};

	/// <summary>
	/// Main problem here is if there's a dupe ship in combined fleet.
	/// Right now it matches via equipment too, but there's a chance both dupes have the same equip.
	/// </summary>
#pragma warning disable IDE0079
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
#pragma warning restore IDE0079
	private IShipData GetShip(ApiShip shipData)
	{
		IEnumerable<IShipData> ships = Fleet.MembersInstance.Where(s => s is not null)!;

		if (EscortFleet is not null)
		{
			ships = ships.Concat(EscortFleet.MembersInstance.Where(s => s is not null))!;
		}

		ships = ships.Where(s => s.MasterID is 0 || s.MasterID == shipData.ApiId);

		if (ships.Count() is 1)
		{
			return ships.First();
		}

		ships = ships.Where(s => s.MasterShip.ShipId == shipData.ApiShipId);

		if (ships.Count() is 1)
		{
			return ships.First();
		}

		ships = ships.Where(s => s.HPCurrent == shipData.ApiNowhp);

		if (ships.Count() is 1)
		{
			return ships.First();
		}

		/*
		 need to fix this first: https://github.com/ElectronicObserverEN/ElectronicObserver/issues/369
		ships = ships.Where(s => s.Level == shipData.ApiLv);

		if (ships.Count() is 1)
		{
			return ships.First();
		}
		*/

		throw new Exception(SortieRecordViewer.DuplicateShipError);
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
