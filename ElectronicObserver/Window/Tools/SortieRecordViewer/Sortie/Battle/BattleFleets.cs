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

	private static int CombinedFleetMainFleetShipCount => 6;

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
		FriendFleet = CloneFleet(FriendFleet),
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

	public IShipData? GetShipByDropId(int? dropId) => dropId switch
	{
		null => null,
		int id => Fleet.MembersWithoutEscaped
			?.FirstOrDefault(s => s?.MasterID == id)
			?? EscortFleet?.MembersWithoutEscaped
				?.FirstOrDefault(s => s?.MasterID == id),
	};

	public IShipData? GetShip(BattleIndex index) => index.FleetFlag switch
	{
		FleetFlag.Player => (EscortFleet is not null && index.Index >= CombinedFleetMainFleetShipCount) switch
		{
			true => EscortFleet!.MembersInstance[index.Index - CombinedFleetMainFleetShipCount],
			_ => Fleet.MembersInstance[index.Index],
		},

		_ => (EnemyEscortFleet is not null && index.Index >= CombinedFleetMainFleetShipCount) switch
		{
			true => EnemyEscortFleet!.MembersInstance[index.Index - CombinedFleetMainFleetShipCount],
			_ => EnemyFleet?.MembersInstance[index.Index],
		},
	};

	public IShipData? GetFriendShip(BattleIndex index) => index.FleetFlag switch
	{
		FleetFlag.Player when FriendFleet is not null => FriendFleet.MembersInstance[index.Index],

		_ => (EnemyEscortFleet is not null && index.Index >= CombinedFleetMainFleetShipCount) switch
		{
			true => EnemyEscortFleet!.MembersInstance[index.Index - CombinedFleetMainFleetShipCount],
			_ => EnemyFleet?.MembersInstance[index.Index],
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

		throw new Exception(SortieRecordViewerResources.DuplicateShipError);
	}

	public void UpdateState(ApiGetMemberShipDeckResponse deck)
	{
		foreach (ApiShip shipData in deck.ApiShipData)
		{
			if (GetShip(shipData) is not ShipDataMock ship) continue;

			ship.Aircraft = shipData.ApiOnslot;
			ship.Condition = shipData.ApiCond;
			ship.Fuel = shipData.ApiFuel;
			ship.Ammo = shipData.ApiBull;
		}
	}

	public void UpdateState(BattleFleets fleetsAfterSortie)
	{
		UpdateFleet(Fleet, fleetsAfterSortie.Fleet);
		UpdateFleet(EscortFleet, fleetsAfterSortie.EscortFleet);
	}

	private static void UpdateFleet(IFleetData? fleet, IFleetData? updateFleet)
	{
		if (fleet is null) return;
		if (updateFleet is null) return;

		foreach ((IShipData? ship, IShipData? updateShip) in fleet.MembersInstance.Zip(updateFleet.MembersInstance))
		{
			if (ship is not ShipDataMock s) continue;
			if (updateShip is not ShipDataMock u) continue;

			s.Aircraft = u.Aircraft;
			s.Condition = u.Condition;
			s.Fuel = u.Fuel;
			s.Ammo = u.Ammo;
		}
	}

	public IFleetData? GetFleet(BattleIndex index) => index.FleetFlag switch
	{
		FleetFlag.Player when index.Index < Fleet.MembersWithoutEscaped?.Count => Fleet,
		FleetFlag.Player => EscortFleet,

		_ when index.Index < EnemyFleet?.MembersWithoutEscaped?.Count => EnemyFleet,
		_ => EnemyEscortFleet,
	};
}
