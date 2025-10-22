using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.AntiAir;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseBaseAirRaid : PhaseBase
{
	public override string Title => BattleRes.BattlePhaseAirBaseRaid;

	private ApiAirBaseRaid AirBattleData { get; }

	public int ApiLostKind { get; }

	public AirState AirState { get; }

	public int Stage1FLostcount { get; }
	public int Stage1FCount { get; }
	public int Stage1ELostcount { get; }
	public int Stage1ECount { get; }

	public int Stage2FLostcount { get; private set; }
	public int Stage2FCount { get; private set; }
	public int Stage2ELostcount { get; private set; }
	public int Stage2ECount { get; private set; }

	/// <summary>
	/// AACI
	/// </summary>
	public ApiAirFire? ApiAirFire { get; private set; }

	private ApiStage1? Stage1 => AirBattleData.ApiStage1;
	private ApiStage2? Stage2 => AirBattleData.ApiStage2;
	private ApiStage3? Stage3 => AirBattleData.ApiStage3;

	private int WaveIndex { get; }

	public List<int> LaunchedShipIndexFriend { get; }
	public List<int> LaunchedShipIndexEnemy { get; }

	private List<AirBattleAttack> Attacks { get; } = new();
	public List<AirBaseRaidAttackViewModel> AttackDisplays { get; } = new();

	public string? Stage1Display { get; }

	public string? TouchAircraftFriend => Stage1?.ApiTouchPlane switch
	{
	[EquipmentId id and > 0, ..] => KCDatabase.Instance.MasterEquipments[(int)id].NameEN,
		_ => null,
	};
	public string? TouchAircraftFriendDisplay => TouchAircraftFriend switch
	{
		string aircraft => $"　{BattleRes.Contact}: {aircraft}",
		_ => null,
	};

	public string? TouchAircraftEnemy => Stage1?.ApiTouchPlane switch
	{
	[_, EquipmentId id and > 0, ..] => KCDatabase.Instance.MasterEquipments[(int)id].NameEN,
		_ => null,
	};
	public string? TouchAircraftEnemyDisplay => TouchAircraftEnemy switch
	{
		string aircraft => $"　{BattleRes.EnemyContact}: {aircraft}",
		_ => null,
	};

	public string? Stage2Display { get; protected set; }

	public List<int> PlayerTorpedoFlags { get; set; } = new();
	public List<int> PlayerBomberFlags { get; set; } = new();
	public List<AirHitType> PlayerHitFlags { get; set; } = new();
	public List<double> PlayerDamage { get; set; } = new();

	public List<int> EnemyTorpedoFlags { get; set; } = new();
	public List<int> EnemyBomberFlags { get; set; } = new();
	public List<AirHitType> EnemyHitFlags { get; set; } = new();
	public List<double> EnemyDamage { get; set; } = new();

	private List<BattleBaseAirCorpsSquadron> Squadrons { get; }

	public string Display { get; }

	public PhaseBaseAirRaid(ApiDestructionBattle battle, int waveIndex = 0)
	{
		AirBattleData = battle.ApiAirBaseAttack;
		WaveIndex = waveIndex;
		ApiLostKind = battle.ApiLostKind;

		if (AirBattleData.ApiStage1 is not null)
		{
			AirState = AirBattleData.ApiStage1.ApiDispSeiku;
			Stage1FLostcount = AirBattleData.ApiStage1.ApiFLostcount;
			Stage1FCount = AirBattleData.ApiStage1.ApiFCount;
			Stage1ELostcount = AirBattleData.ApiStage1.ApiELostcount;
			Stage1ECount = AirBattleData.ApiStage1.ApiECount;

			Stage1Display = GetStage1Display
			(
				AirState,
				Stage1FLostcount,
				Stage1FCount,
				Stage1ELostcount,
				Stage1ECount
			);
		}

		LaunchedShipIndexFriend = GetLaunchedShipIndex(AirBattleData.ApiPlaneFrom, 0);
		LaunchedShipIndexEnemy = GetLaunchedShipIndex(AirBattleData.ApiPlaneFrom, 1);

		Squadrons = AirBattleData.ApiMapSquadronPlane?.Values
			.SelectMany(b => b)
			.Select(b => new BattleBaseAirCorpsSquadron
			{
				Equipment = KCDatabase.Instance.MasterEquipments[(int)b.ApiMstId],
				AircraftCount = b.ApiCount,
			}).ToList()
			?? new();

		StringBuilder sb = new();

		sb.AppendLine(ConstantsRes.BattleDetail_AirAttackUnits);
		sb.Append("　").Append(string.Join(", ", Squadrons
			.Where(sq => sq.Equipment is not null)
			.Select(sq => sq.ToString())
			.DefaultIfEmpty(BattleRes.Empty)));

		Display = sb.ToString();
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();
		FleetsAfterPhase = battleFleets;

		if (Stage2 is { } stage2)
		{
			Stage2FLostcount = stage2.ApiFLostcount;
			Stage2FCount = stage2.ApiFCount;
			Stage2ELostcount = stage2.ApiELostcount;
			Stage2ECount = stage2.ApiECount;
			ApiAirFire = stage2.ApiAirFire;

			StringBuilder sb = new();
			sb.Append("Stage 2:");

			if (ApiAirFire is not null)
			{
				sb.AppendFormat(BattleRes.AaciType,
					FleetsAfterPhase.GetShip(new(ApiAirFire.ApiIdx, FleetFlag.Player))?.NameWithLevel,
					AntiAirCutIn.FromId(ApiAirFire.ApiKind).EquipmentConditionsSingleLineDisplay(),
					ApiAirFire.ApiKind);
			}

			sb.AppendLine();
			sb.AppendLine($"　{BattleRes.Friendly}: -{Stage2FLostcount}/{Stage2FCount}");
			sb.Append($"　{BattleRes.Enemy}: -{Stage2ELostcount}/{Stage2ECount}");

			Stage2Display = sb.ToString();
		}

		ApiStage3? stage3 = Stage3;

		if (stage3 is not null)
		{
			PlayerTorpedoFlags = stage3.ApiFraiFlag.Select(i => i ?? 0).ToList();
			PlayerBomberFlags = stage3.ApiFbakFlag.Select(i => i ?? 0).ToList();
			PlayerHitFlags = stage3.ApiFclFlag;
			PlayerDamage = stage3.ApiFdam;

			Attacks.AddRange(GetAttacks(FleetFlag.Player, 0, FleetsAfterPhase.Fleet,
				PlayerTorpedoFlags,
				PlayerBomberFlags,
				PlayerHitFlags,
				PlayerDamage));
		}

		if (stage3 is not null)
		{
			EnemyTorpedoFlags = stage3.ApiEraiFlag;
			EnemyBomberFlags = stage3.ApiEbakFlag;
			EnemyHitFlags = stage3.ApiEclFlag;
			EnemyDamage = stage3.ApiEdam;

			Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 0, FleetsAfterPhase.EnemyFleet,
				EnemyTorpedoFlags,
				EnemyBomberFlags,
				EnemyHitFlags,
				EnemyDamage));
		}

		foreach (AirBattleAttack attack in Attacks.Where(attack => attack.AttackType is not AirAttack.None))
		{
			AttackDisplays.Add(new(FleetsAfterPhase, WaveIndex, attack));
			AddAirBaseDamage(FleetsAfterPhase, attack.Defenders.First().Defender, attack.Defenders.First().Damage);
		}


		return FleetsAfterPhase.Clone();
	}
}
