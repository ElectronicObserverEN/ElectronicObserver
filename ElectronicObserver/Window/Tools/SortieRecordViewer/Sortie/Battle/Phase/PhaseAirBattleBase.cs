using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseAirBattleBase : PhaseBase
{
	public override string Title => BattleRes.BattlePhaseAirBattle;

	/// <summary>
	/// <see cref="IApiAirBattle" /> or <see cref="IApiJetAirBattle" />
	/// </summary>
	private object AirBattleData { get; }

	private ApiStage1? Stage1 => AirBattleData switch
	{
		IApiAirBattle aab => aab.ApiStage1,
		_ => null,
	};

	private ApiStage2? Stage2 => AirBattleData switch
	{
		IApiAirBattle aab => aab.ApiStage2,
		_ => null,
	};

	private ApiStage1And2Jet? Stage2Jet => AirBattleData switch
	{
		IApiJetAirBattle jet => jet.ApiStage2,
		_ => null,
	};

	private ApiStage3? Stage3 => AirBattleData switch
	{
		IApiAirBattle aab => aab.ApiStage3,
		_ => null,
	};

	private ApiStage3Jet? Stage3Jet => AirBattleData switch
	{
		IApiJetAirBattle jet => jet.ApiStage3,
		_ => null,
	};

	private ApiStage3Combined? Stage3Combined => AirBattleData switch
	{
		IApiAirBattle aab => aab.ApiStage3Combined,
		_ => null,
	};

	private ApiStage3JetCombined? Stage3JetCombined => AirBattleData switch
	{
		IApiJetAirBattle jet => jet.ApiStage3Combined,
		_ => null,
	};

	protected int WaveIndex { get; }

	public List<int> LaunchedShipIndexFriend { get; }
	public List<int> LaunchedShipIndexEnemy { get; }

	private List<AirBattleAttack> Attacks { get; } = new();
	public List<AirBattleAttackViewModel> AttackDisplays { get; } = new();

	public string? Stage1Display { get; }

	public string? TouchAircraftFriend => Stage1?.ApiTouchPlane switch
	{
		[EquipmentId id and > 0, ..] => $"　{BattleRes.Contact}: {KCDatabase.Instance.MasterEquipments[(int)id].NameEN}",
		_ => null,
	};

	public string? TouchAircraftEnemy => Stage1?.ApiTouchPlane switch
	{
		[_, EquipmentId id and > 0, ..] => $"　{BattleRes.EnemyContact}: {KCDatabase.Instance.MasterEquipments[(int)id].NameEN}",
		_ => null,
	};

	public string? Stage2Display { get; private set; }

	protected PhaseAirBattleBase(IApiAirBattle airBattleData, int waveIndex = 0)
	{
		AirBattleData = airBattleData;
		WaveIndex = waveIndex;

		if (airBattleData.ApiStage1 is not null)
		{
			Stage1Display = GetStage1Display
			(
				airBattleData.ApiStage1.ApiDispSeiku,
				airBattleData.ApiStage1.ApiFLostcount,
				airBattleData.ApiStage1.ApiFCount,
				airBattleData.ApiStage1.ApiELostcount,
				airBattleData.ApiStage1.ApiECount
			);
		}

		LaunchedShipIndexFriend = GetLaunchedShipIndex(airBattleData.ApiPlaneFrom, 0);
		LaunchedShipIndexEnemy = GetLaunchedShipIndex(airBattleData.ApiPlaneFrom, 1);
	}

	protected PhaseAirBattleBase(IApiJetAirBattle airBattleData, int waveIndex = 0)
	{
		AirBattleData = airBattleData;
		WaveIndex = waveIndex;

		if (airBattleData.ApiStage1 is not null)
		{
			Stage1Display = GetStage1Display
			(
				AirState.Unknown,
				airBattleData.ApiStage1.ApiFLostcount,
				airBattleData.ApiStage1.ApiFCount,
				airBattleData.ApiStage1.ApiELostcount,
				airBattleData.ApiStage1.ApiECount
			);
		}

		LaunchedShipIndexFriend = GetLaunchedShipIndex(airBattleData.ApiPlaneFrom, 0);
		LaunchedShipIndexEnemy = GetLaunchedShipIndex(airBattleData.ApiPlaneFrom, 1);
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();

		if (Stage2 is { } stage2)
		{
			StringBuilder sb = new();

			sb.Append("Stage 2:");
			if (stage2.ApiAirFire is not null)
			{
				sb.AppendFormat(BattleRes.AaciType,
					battleFleets.GetShip(new(stage2.ApiAirFire.ApiIdx, FleetFlag.Player))?.NameWithLevel,
					AntiAirCutIn.FromId(stage2.ApiAirFire.ApiKind).EquipmentConditionsSingleLineDisplay(),
					stage2.ApiAirFire.ApiKind);
			}
			sb.AppendLine();
			sb.AppendLine($"　{BattleRes.Friendly}: -{stage2.ApiFLostcount}/{stage2.ApiFCount}");
			sb.Append($"　{BattleRes.Enemy}: -{stage2.ApiELostcount}/{stage2.ApiECount}");

			Stage2Display = sb.ToString();
		}
		else if (Stage2Jet is { } stage2Jet)
		{
			StringBuilder sb = new();

			sb.Append("Stage 2:");
			sb.AppendLine();
			sb.AppendLine($"　{BattleRes.Friendly}: -{stage2Jet.ApiFLostcount}/{stage2Jet.ApiFCount}");
			sb.Append($"　{BattleRes.Enemy}: -{stage2Jet.ApiELostcount}/{stage2Jet.ApiECount}");

			Stage2Display = sb.ToString();
		}

		ApiStage3? stage3 = Stage3;
		ApiStage3Jet? stage3Jet = Stage3Jet;
		ApiStage3Combined? stage3Combined = Stage3Combined;
		ApiStage3JetCombined? stage3JetCombined = Stage3JetCombined;

		if (stage3 is not null)
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Player, 0, battleFleets.Fleet,
				stage3.ApiFraiFlag.Select(i => i ?? 0).ToList(),
				stage3.ApiFbakFlag.Select(i => i ?? 0).ToList(),
				stage3.ApiFclFlag,
				stage3.ApiFdam));
		}

		if (stage3Jet is not null)
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 0, battleFleets.Fleet,
				stage3Jet.ApiEraiFlag,
				stage3Jet.ApiEbakFlag,
				stage3Jet.ApiEclFlag,
				stage3Jet.ApiEdam));
		}

		if (stage3Combined is { ApiFraiFlag: not null, ApiFbakFlag: not null, ApiFclFlag: not null, ApiFdam: not null })
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Player, 6, battleFleets.EscortFleet,
				stage3Combined.ApiFraiFlag.Select(i => i ?? 0).ToList(),
				stage3Combined.ApiFbakFlag.Select(i => i ?? 0).ToList(),
				stage3Combined.ApiFclFlag,
				stage3Combined.ApiFdam));
		}

		if (stage3JetCombined is { ApiEraiFlag: not null, ApiEbakFlag: not null, ApiEclFlag: not null, ApiEdam: not null })
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 6, battleFleets.EscortFleet,
				stage3JetCombined.ApiEraiFlag,
				stage3JetCombined.ApiEbakFlag,
				stage3JetCombined.ApiEclFlag,
				stage3JetCombined.ApiEdam));
		}

		if (stage3 is not null)
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 0, battleFleets.EnemyFleet,
				stage3.ApiEraiFlag,
				stage3.ApiEbakFlag,
				stage3.ApiEclFlag,
				stage3.ApiEdam));
		}

		if (stage3Combined is { ApiEraiFlag: not null, ApiEbakFlag: not null, ApiEclFlag: not null, ApiEdam: not null })
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 6, battleFleets.EnemyEscortFleet,
				stage3Combined.ApiEraiFlag,
				stage3Combined.ApiEbakFlag,
				stage3Combined.ApiEclFlag,
				stage3Combined.ApiEdam));
		}

		foreach (AirBattleAttack attack in Attacks.Where(attack => attack.AttackType is not AirAttack.None))
		{
			AttackDisplays.Add(new(battleFleets, WaveIndex, attack));
			AddDamage(battleFleets, attack.Defenders.First().Defender, attack.Defenders.First().Damage);
		}

		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}
}
