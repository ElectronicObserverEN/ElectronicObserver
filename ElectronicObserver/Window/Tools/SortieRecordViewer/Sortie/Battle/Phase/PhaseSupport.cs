using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Properties.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseSupport : PhaseBase
{
	public override bool IsAvailable => true;

	private SupportType ApiSupportFlag { get; }
	private bool IsNightSupport { get; }

	public string Title => IsNightSupport switch
	{
		true => BattleRes.BattlePhaseNightSupportExpedition,
		_ => BattleRes.BattlePhaseSupportExpedition,
	};

	private List<double> Damages { get; }
	private List<HitType> Criticals { get; }

	private List<PhaseSupportAttack> Attacks { get; } = new();
	public List<PhaseSupportAttackViewModel> AttackDisplays { get; } = new();

	public PhaseSupport(SupportType apiSupportFlag, ApiSupportInfo apiSupportInfo, bool isNightSupport)
	{
		ApiSupportFlag = apiSupportFlag;
		IsNightSupport = isNightSupport;

		Damages = apiSupportFlag switch
		{
			SupportType.Aerial or SupportType.AntiSubmarine
				when apiSupportInfo.ApiSupportAiratack is { } attack && attack.ApiStageFlag[2] is not 0
				=> attack.ApiStage3.ApiEdam,

			SupportType.Shelling or SupportType.Torpedo when apiSupportInfo.ApiSupportHourai is { } attack
				=> attack.ApiDamage,

			_ => new(),
		};

		Criticals = apiSupportFlag switch
		{
			SupportType.Aerial or SupportType.AntiSubmarine
				when apiSupportInfo.ApiSupportAiratack is { } attack && attack.ApiStageFlag[2] is not 0
				=> attack.ApiStage3.ApiEclFlag.Select(h => h switch
				{
					AirHitType.Critical => HitType.Critical,
					_ => HitType.Hit,
				}).ToList(),

			SupportType.Shelling or SupportType.Torpedo when apiSupportInfo.ApiSupportHourai is { } attack
				=> attack.ApiClList,

			_ => new(),
		};
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();

		int mainFleetLimit = battleFleets.EnemyFleet!.MembersInstance.Count(s => s is not null);
		mainFleetLimit = Math.Min(mainFleetLimit, Damages.Count);
		mainFleetLimit = Math.Min(mainFleetLimit, Criticals.Count);

		for (int i = 0; i < mainFleetLimit; i++)
		{
			Attacks.Add(new()
			{
				AttackType = ApiSupportFlag,
				Defenders = new()
				{
					new()
					{
						Defender = new(i, FleetFlag.Enemy),
						RawDamage = Damages[i],
						CriticalFlag = Criticals[i],
					},
				},
			});
		}

		if (battleFleets.EnemyEscortFleet is not null)
		{
			int escortFleetLimit = battleFleets.EnemyEscortFleet.MembersInstance.Count(s => s is not null);
			escortFleetLimit = Math.Min(escortFleetLimit, Damages.Count);
			escortFleetLimit = Math.Min(escortFleetLimit, Criticals.Count);

			for (int i = 0; i < escortFleetLimit; i++)
			{
				Attacks.Add(new()
				{
					AttackType = ApiSupportFlag,
					Defenders = new()
					{
						new()
						{
							Defender = new(i + 6, FleetFlag.Enemy),
							RawDamage = Damages[i + 6],
							CriticalFlag = Criticals[i + 6],
						},
					},
				});
			}
		}

		foreach (PhaseSupportAttack atk in Attacks)
		{
			foreach (IGrouping<BattleIndex, PhaseSupportDefender> defs in atk.Defenders.GroupBy(d => d.Defender))
			{
				AttackDisplays.Add(new PhaseSupportAttackViewModel(battleFleets, atk));
				AddDamage(battleFleets, defs.Key, defs.Sum(d => d.Damage));
			}
		}

		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}
}
