using System.Diagnostics.CodeAnalysis;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public class BattleFleets
{
	public IFleetData Fleet { get; }
	public IFleetData? EscortFleet { get; set; }
	public IFleetData? EnemyFleet { get; set; }
	public IFleetData? EnemyEscortFleet { get; set; }

	public BattleFleets(IFleetData fleet)
	{
		Fleet = fleet;
	}

	public BattleFleets Clone() => new(CloneFleet(Fleet))
	{
		EscortFleet = CloneFleet(EscortFleet),
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
