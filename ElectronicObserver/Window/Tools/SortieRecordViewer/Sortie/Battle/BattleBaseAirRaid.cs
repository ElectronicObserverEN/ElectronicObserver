using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public sealed class BattleBaseAirRaid : BattleData
{
	private PhaseBaseAirRaid? BaseAirRaid { get; }

	public BattleBaseAirRaid(IKCDatabase kcDatabase, BattleFleets fleets, ApiDestructionBattle battle)
		: base(kcDatabase, fleets, battle)
	{
		BaseAirRaid = GetBaseAirRaidPhase(battle.ApiAirBaseAttack);

		foreach (PhaseBase phase in Phases)
		{
			FleetsAfterBattle = phase.EmulateBattle(FleetsAfterBattle);
		}
	}

	protected override IEnumerable<PhaseBase?> AllPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return BaseAirRaid;
	}

	private static PhaseBaseAirRaid? GetBaseAirRaidPhase(ApiAirBaseRaid? a) => a switch
	{
		null => null,
		_ => new(a),
	};
}
