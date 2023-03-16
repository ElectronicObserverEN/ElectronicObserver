using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseBase
{
	public virtual bool IsAvailable { get; } = true;

	public BattleFleets? FleetsBeforePhase { get; protected set; }
	public BattleFleets? FleetsAfterPhase { get; protected set; }

	public virtual BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();
		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}

	protected void AddDamage(BattleFleets fleets, BattleIndex index, int damage)
	{
		IShipData? ship = fleets.GetShip(index);

		if (ship is ShipDataMock s)
		{
			s.HPCurrent -= damage;
		}
	}
}
