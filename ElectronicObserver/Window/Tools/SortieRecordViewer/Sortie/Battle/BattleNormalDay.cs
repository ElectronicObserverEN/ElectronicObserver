using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public sealed class BattleNormalDay : BattleDay
{
	public BattleNormalDay(IKCDatabase kcDatabase, BattleFleets fleets, ApiReqSortieBattleResponse battle)
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
		yield return Shelling3;
		yield return Torpedo;
	}
}
