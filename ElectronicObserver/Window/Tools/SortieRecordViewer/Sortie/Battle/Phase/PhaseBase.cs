using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public abstract class PhaseBase
{
	public abstract string Title { get; }

	public BattleFleets? FleetsBeforePhase { get; protected set; }
	public BattleFleets? FleetsAfterPhase { get; protected set; }

	public virtual BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();
		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}

	protected static void AddDamage(BattleFleets fleets, BattleIndex index, int damage)
	{
		IShipData? ship = fleets.GetShip(index);

		if (ship is ShipDataMock s)
		{
			s.HPCurrent -= damage;
		}
	}

	protected static void AddFriendDamage(BattleFleets fleets, BattleIndex index, int damage)
	{
		IShipData? ship = fleets.GetFriendShip(index);

		if (ship is ShipDataMock s)
		{
			s.HPCurrent -= damage;
		}
	}
}
