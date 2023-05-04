using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Properties.Data;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseAirBattleBase : PhaseBase
{
	/// <summary>
	/// <see cref="IApiAirBattle" /> or <see cref="ApiInjectionKouku" />
	/// </summary>
	private object AirBattleData { get; }

	private ApiStage1? Stage1 => AirBattleData switch
	{
		IApiAirBattle aab => aab.ApiStage1,
		ApiInjectionKouku jet => jet.ApiStage1,
		_ => null,
	};

	private ApiStage2? Stage2 => AirBattleData switch
	{
		IApiAirBattle aab => aab.ApiStage2,
		_ => null,
	};

	private ApiStage2Jet? Stage2Jet => AirBattleData switch
	{
		ApiInjectionKouku jet => jet.ApiStage2,
		_ => null,
	};

	private ApiStage3? Stage3 => AirBattleData switch
	{
		IApiAirBattle aab => aab.ApiStage3,
		ApiInjectionKouku jet => jet.ApiStage3,
		_ => null,
	};

	private ApiStage3? Stage3Combined => AirBattleData switch
	{
		IApiAirBattle aab => aab.ApiStage3Combined,
		ApiInjectionKouku jet => jet.ApiStage3Combined,
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

	protected PhaseAirBattleBase(ApiInjectionKouku apiInjectionKouku)
	{
		AirBattleData = apiInjectionKouku;

		if (apiInjectionKouku.ApiStage1 is not null)
		{
			Stage1Display = GetStage1Display
			(
				AirState.Unknown,
				apiInjectionKouku.ApiStage1.ApiFLostcount,
				apiInjectionKouku.ApiStage1.ApiFCount,
				apiInjectionKouku.ApiStage1.ApiELostcount,
				apiInjectionKouku.ApiStage1.ApiECount
			);
		}

		LaunchedShipIndexFriend = GetLaunchedShipIndex(apiInjectionKouku.ApiPlaneFrom, 0);
		LaunchedShipIndexEnemy = GetLaunchedShipIndex(apiInjectionKouku.ApiPlaneFrom, 1);
	}

	private static List<int> GetLaunchedShipIndex(List<List<int>?> apiPlaneFrom, int index) =>
		apiPlaneFrom
			.Skip(index)
			.FirstOrDefault()
			?.Where(i => i > 0)
			.Select(i => i - 1)
			.ToList()
		?? new();

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
		ApiStage3? stage3Combined = Stage3Combined;

		if (stage3 is not null)
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Player, 0, battleFleets.Fleet,
				stage3.ApiFraiFlag.Select(i => i ?? 0).ToList(),
				stage3.ApiFbakFlag.Select(i => i ?? 0).ToList(),
				stage3.ApiFclFlag,
				stage3.ApiFdam));
		}

		if (stage3Combined is not null)
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Player, 6, battleFleets.EscortFleet,
				stage3Combined.ApiFraiFlag.Select(i => i ?? 0).ToList(),
				stage3Combined.ApiFbakFlag.Select(i => i ?? 0).ToList(),
				stage3Combined.ApiFclFlag,
				stage3Combined.ApiFdam));
		}

		if (stage3 is not null)
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 0, battleFleets.EnemyFleet,
				stage3.ApiEraiFlag,
				stage3.ApiEbakFlag,
				stage3.ApiEclFlag,
				stage3.ApiEdam));
		}

		if (stage3Combined is not null)
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

	private List<AirBattleAttack> GetAttacks(FleetFlag fleetFlag, int indexOffset, IFleetData? fleet,
		List<int> torpedoFlags, List<int> bomberFlags, List<AirHitType> criticalFlags, List<double> damages)
		=> fleet?.MembersInstance
			.Select((s, i) => (Ship: s, Index: i + indexOffset))
			.Zip(torpedoFlags, (t, f) => (t.Ship, t.Index, TorpedoFlag: f))
			.Zip(bomberFlags, (t, b) => (t.Ship, t.Index, t.TorpedoFlag, BomberFlag: b))
			.Zip(criticalFlags, (t, c) => (t.Ship, t.Index, t.TorpedoFlag, t.BomberFlag, CriticalFlag: c))
			.Zip(damages, (t, d) => (t.Ship, t.Index, t.TorpedoFlag, t.BomberFlag, t.CriticalFlag, Damage: d))
			.Select(t => new AirBattleAttack
			{
				AttackType = (t.TorpedoFlag, t.BomberFlag) switch
				{
					( > 0, > 0) => AirAttack.TorpedoBombing,
					( > 0, _) => AirAttack.Torpedo,
					(_, > 0) => AirAttack.Bombing,

					_ => AirAttack.None,
				},
				Defenders = new List<AirBattleDefender>
				{
					new()
					{
						Defender = new BattleIndex(t.Index, fleetFlag),
						CriticalFlag = (t.CriticalFlag, t.Damage) switch
						{
							(AirHitType.Critical, _) => HitType.Critical,
							(AirHitType.HitOrMiss, > 0) => HitType.Hit,
							(AirHitType.HitOrMiss, _) => HitType.Miss,

							_ => HitType.Invalid,
						},
						RawDamage = t.Damage,
					},
				},
			}).ToList()
			?? new();

	private string GetStage1Display(AirState airState, int friendlyLost, int friendlyTotal,
		int enemyLost, int enemyTotal) =>
		$"""
		Stage 1: {Constants.GetAirSuperiority(airState)}
		　{BattleRes.Friendly}: -{friendlyLost}/{friendlyTotal}
		　{BattleRes.Enemy}: -{enemyLost}/{enemyTotal}
		""";
}
