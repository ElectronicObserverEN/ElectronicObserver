using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public sealed class BattleEnemyCombinedDay : BattleDay
{
	public BattleEnemyCombinedDay(IKCDatabase kcDatabase, BattleFleets fleets, ApiReqCombinedBattleEcBattleResponse battle)
		: base(kcDatabase, fleets, battle)
	{
		foreach (PhaseBase phase in Phases)
		{
			FleetsAfterBattle = phase.EmulateBattle(FleetsAfterBattle);
		}
	}

	protected override IEnumerable<PhaseBase?> AllPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return JetBaseAirAttack;
		yield return JetAirBattle;
		yield return BaseAirAttack;
		yield return FriendlySupportInfo;
		yield return FriendlyAirBattle;
		yield return AirBattle;
		yield return Support;
		yield return OpeningAsw;
		yield return OpeningTorpedo;
		yield return Shelling1;
		yield return Torpedo;
		yield return Shelling2;
		yield return Shelling3;
	}
}
