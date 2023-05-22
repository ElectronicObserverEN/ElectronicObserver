using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ElectronicObserverTypes;

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
}
