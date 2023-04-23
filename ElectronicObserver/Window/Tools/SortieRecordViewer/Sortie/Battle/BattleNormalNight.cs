using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public class BattleNormalNight : BattleNight
{
	public BattleNormalNight(IKCDatabase kcDatabase, BattleFleets fleets, INightBattleApiResponse battle)
		: base(kcDatabase, fleets, battle)
	{
		foreach (PhaseBase phase in GetPhases())
		{
			FleetsAfterBattle = phase.EmulateBattle(FleetsAfterBattle);
		}
	}

	public override IEnumerable<PhaseBase> Phases => GetPhases();

	private IEnumerable<PhaseBase> GetPhases()
	{
		yield return Initial;
		yield return NightInitial;
		yield return FriendlySupportInfo;
		yield return FriendlyShelling;
		yield return NightBattle;
	}
}
