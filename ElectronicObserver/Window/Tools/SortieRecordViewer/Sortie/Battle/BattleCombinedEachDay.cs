using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public sealed class BattleCombinedEachDay : BattleDay
{
	public BattleCombinedEachDay(IKCDatabase kcDatabase, BattleFleets fleets, ApiReqCombinedBattleEachBattleResponse battle)
		: base(kcDatabase, fleets, battle)
	{
		foreach (PhaseBase phase in Phases)
		{
			FleetsAfterBattle = phase.EmulateBattle(FleetsAfterBattle);
		}
	}

	public override IEnumerable<PhaseBase> Phases => GetPhases().Where(p => p?.IsAvailable is true)!;

	private IEnumerable<PhaseBase?> GetPhases()
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
		yield return Shelling2;
		yield return Torpedo;
		yield return Shelling3;
	}
}
