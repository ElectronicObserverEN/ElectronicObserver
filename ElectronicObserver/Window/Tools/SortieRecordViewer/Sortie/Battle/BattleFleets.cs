using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public class BattleFleets
{
	public IFleetData Fleet { get; }
	public IFleetData? EscortFleet { get; }
	public List<IFleetData?>? Fleets { get; }
	public List<IBaseAirCorpsData> AirBases { get; }
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

	public BattleFleets Clone() => new(CloneFleet(Fleet), CloneFleet(EscortFleet), Fleets, AirBases)
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
}
