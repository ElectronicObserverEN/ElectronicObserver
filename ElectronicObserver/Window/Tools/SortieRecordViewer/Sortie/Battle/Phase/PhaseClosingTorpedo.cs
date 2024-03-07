using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public sealed class PhaseClosingTorpedo(ApiRaigekiClass apiRaigekiClass) : PhaseTorpedo
{
	public override string Title => BattleRes.BattlePhaseClosingTorpedo;

	private ApiRaigekiClass ApiRaigekiClass { get; } = apiRaigekiClass;

	private List<PhaseTorpedoAttack> Attacks { get; } = [];

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();
		FleetsAfterPhase = battleFleets;

		Attacks.AddRange(ProcessPlayerAttacks(ApiRaigekiClass));
		Attacks.AddRange(ProcessEnemyAttacks(ApiRaigekiClass));

		foreach (PhaseTorpedoAttack attack in Attacks)
		{
			AttackDisplays.Add(new(FleetsAfterPhase, attack));
			AddDamage(FleetsAfterPhase, attack.Defenders.First().Defender, attack.Defenders.First().Damage);
		}

		return FleetsAfterPhase.Clone();
	}
}
