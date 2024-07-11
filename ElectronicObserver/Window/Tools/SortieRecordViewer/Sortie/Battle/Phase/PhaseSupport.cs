using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseSupport : PhaseBase
{
	public override string Title => IsNightSupport switch
	{
		true => BattleRes.BattlePhaseNightSupportExpedition,
		_ => BattleRes.BattlePhaseSupportExpedition,
	};

	public int? Stage1FLostcount { get; }
	public int? Stage1FCount { get; }

	public int? Stage2FLostcount { get; }
	public int? Stage2FCount { get; }

	public SupportType SupportFlag { get; }
	private bool IsNightSupport { get; }

	private List<double> Damages { get; }
	private List<HitType> Criticals { get; }
	private int SupportFleetId { get; }
	private IFleetData? SupportFleet { get; set; }

	public string? SupportFleetDisplay => CreateDisplay();

	private List<PhaseSupportAttack> Attacks { get; } = new();
	public List<PhaseSupportAttackViewModel> AttackDisplays { get; } = new();

	public PhaseSupport(SupportType supportFlag, ApiSupportInfo apiSupportInfo, bool isNightSupport)
	{
		SupportFlag = supportFlag;
		IsNightSupport = isNightSupport;

		static object? SupportAttack(SupportType apiSupportFlag, ApiSupportInfo apiSupportInfo) => apiSupportFlag switch
		{
			SupportType.Aerial or SupportType.AntiSubmarine => apiSupportInfo.ApiSupportAiratack,
			SupportType.Shelling or SupportType.Torpedo => apiSupportInfo.ApiSupportHourai,
			_ => null,
		};

		Damages = SupportAttack(supportFlag, apiSupportInfo) switch
		{
			ApiSupportAiratack attack when attack.ApiStageFlag[2] is not 0
				=> attack.ApiStage3.ApiEdam,

			ApiSupportHourai attack => attack.ApiDamage,

			_ => new(),
		};

		Criticals = SupportAttack(supportFlag, apiSupportInfo) switch
		{
			ApiSupportAiratack attack when attack.ApiStageFlag[2] is not 0
				=> attack.ApiStage3.ApiEclFlag.Select(h => h switch
				{
					AirHitType.Critical => HitType.Critical,
					_ => HitType.Hit,
				}).ToList(),

			ApiSupportHourai attack => attack.ApiClList,

			_ => new(),
		};

		SupportFleetId = SupportAttack(supportFlag, apiSupportInfo) switch
		{
			ApiSupportAiratack attack => attack.ApiDeckId,
			ApiSupportHourai attack => attack.ApiDeckId,

			_ => -1,
		};

		if (apiSupportInfo.ApiSupportAiratack is not null)
		{
			Stage1FCount = apiSupportInfo.ApiSupportAiratack.ApiStage1.ApiFCount;
			Stage1FLostcount = apiSupportInfo.ApiSupportAiratack.ApiStage1.ApiFLostcount;

			Stage2FCount = apiSupportInfo.ApiSupportAiratack.ApiStage2.ApiFCount;
			Stage2FLostcount = apiSupportInfo.ApiSupportAiratack.ApiStage2.ApiFLostcount;
		}
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets, List<int> damages)
	{
		FleetsBeforePhase = battleFleets.Clone();
		FleetsAfterPhase = battleFleets;

		if (SupportFleetId is not -1)
		{
			SupportFleet = FleetsAfterPhase.Fleets?.Skip(SupportFleetId - 1).FirstOrDefault();
		}

		int mainFleetLimit = FleetsAfterPhase.EnemyFleet!.MembersInstance.Count(s => s is not null);
		mainFleetLimit = Math.Min(mainFleetLimit, Damages.Count);
		mainFleetLimit = Math.Min(mainFleetLimit, Criticals.Count);

		for (int i = 0; i < mainFleetLimit; i++)
		{
			Attacks.Add(new()
			{
				AttackType = SupportFlag,
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

		if (FleetsAfterPhase.EnemyEscortFleet is not null)
		{
			int escortFleetLimit = FleetsAfterPhase.EnemyEscortFleet.MembersInstance.Count(s => s is not null);
			escortFleetLimit = Math.Min(escortFleetLimit, Damages.Count);
			escortFleetLimit = Math.Min(escortFleetLimit, Criticals.Count);

			for (int i = 0; i < escortFleetLimit; i++)
			{
				Attacks.Add(new()
				{
					AttackType = SupportFlag,
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
				AttackDisplays.Add(new PhaseSupportAttackViewModel(FleetsAfterPhase, atk));
				AddDamage(FleetsAfterPhase, defs.Key, defs.Sum(d => d.Damage));
			}
		}

		return FleetsAfterPhase.Clone();
	}

	private string? CreateDisplay()
	{
		if (SupportFleet is null) return null;

		StringBuilder sb = new();

		sb.AppendLine($"〈{BattleRes.SupportFleet}〉");

		for (int i = 0; i < SupportFleet.MembersInstance.Count; i++)
		{
			IShipData? ship = SupportFleet.MembersInstance[i];

			if (ship is null) continue;

			sb.AppendFormat($"#{{0}}: {{1}} {{2}} " +
							$"Lv. {{3}} " +
							$"HP: {{4}} / {{5}} - " +
							$"{GeneralRes.Firepower} {{6}}, " +
							$"{GeneralRes.Torpedo} {{7}}, " +
							$"{GeneralRes.AntiAir} {{8}}, " +
							$"{GeneralRes.Armor} {{9}}" +
							$"\r\n",
				i + 1,
				ship.MasterShip.ShipTypeName, ship.MasterShip.NameWithClass,
				ship.Level,
				ship.HPCurrent, ship.HPMax,
				ship.FirepowerBase, ship.TorpedoBase, ship.AABase, ship.ArmorBase);

			sb.Append("　");
			sb.AppendLine(string.Join(", ", ship.AllSlotInstance
				.Where(eq => eq is not null)
				.Select(eq => eq!.Name)));
		}

		return sb.ToString();
	}
}
