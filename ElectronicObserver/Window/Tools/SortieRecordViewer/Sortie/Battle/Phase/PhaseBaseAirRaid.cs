using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Interfaces;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseBaseAirRaid : PhaseBase, IPhaseAirBattle
{
	public override string Title => BattleRes.BattlePhaseAirBaseRaid;

	private ApiAirBaseRaid AirBattleData { get; }

	public int ApiLostKind { get; }

	public AirState AirState { get; }

	public int AircraftLostStage1Friend { get; }
	public int AircraftTotalStage1Friend { get; }
	public int AircraftLostStage1Enemy { get; }
	public int AircraftTotalStage1Enemy { get; }

	public int AircraftLostStage2Friend { get; private set; }
	public int AircraftTotalStage2Friend { get; private set; }
	public int AircraftLostStage2Enemy { get; private set; }
	public int AircraftTotalStage2Enemy { get; private set; }

	[MemberNotNullWhen(true, nameof(ApiAirFire))]
	public bool IsAACutinAvailable => ApiAirFire is not null;

	public int AACutInIndex => ApiAirFire?.ApiIdx ?? -1;
	public string? AACutInShipName => IsAACutinAvailable switch
	{
		true => FleetsAfterPhase?.GetShip(new(ApiAirFire.ApiIdx, FleetFlag.Player))?.NameWithLevel,
		_ => null,
	};
	public int AACutInKind => ApiAirFire?.ApiKind ?? -1;

	/// <summary>
	/// AACI
	/// </summary>
	public ApiAirFire? ApiAirFire { get; private set; }

	[MemberNotNullWhen(true, nameof(Stage1))]
	public bool IsStage1Available => Stage1 is not null;

	private ApiStage1? Stage1 => AirBattleData.ApiStage1;

	[MemberNotNullWhen(true, nameof(Stage2))]
	public bool IsStage2Available => Stage2 is not null;

	private ApiStage2? Stage2 => AirBattleData.ApiStage2;

	[MemberNotNullWhen(true, nameof(Stage3))]
	public bool IsStage3Available => Stage3 is not null;

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

	public List<BattleBaseAirCorpsSquadron> Squadrons { get; }

	public string Display { get; }

	public PhaseBaseAirRaid(ApiDestructionBattle battle, int waveIndex = 0)
	{
		AirBattleData = battle.ApiAirBaseAttack;
		WaveIndex = waveIndex;
		ApiLostKind = battle.ApiLostKind;

		if (AirBattleData.ApiStage1 is not null)
		{
			AirState = AirBattleData.ApiStage1.ApiDispSeiku;
			AircraftLostStage1Friend = AirBattleData.ApiStage1.ApiFLostcount;
			AircraftTotalStage1Friend = AirBattleData.ApiStage1.ApiFCount;
			AircraftLostStage1Enemy = AirBattleData.ApiStage1.ApiELostcount;
			AircraftTotalStage1Enemy = AirBattleData.ApiStage1.ApiECount;

			Stage1Display = GetStage1Display
			(
				AirState,
				AircraftLostStage1Friend,
				AircraftTotalStage1Friend,
				AircraftLostStage1Enemy,
				AircraftTotalStage1Enemy
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

	public override BattleFleets EmulateBattle(BattleFleets battleFleets, List<int> damages)
	{
		FleetsBeforePhase = battleFleets.Clone();
		FleetsAfterPhase = battleFleets;

		if (Stage2 is { } stage2)
		{
			AircraftLostStage2Friend = stage2.ApiFLostcount;
			AircraftTotalStage2Friend = stage2.ApiFCount;
			AircraftLostStage2Enemy = stage2.ApiELostcount;
			AircraftTotalStage2Enemy = stage2.ApiECount;
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
			sb.AppendLine($"　{BattleRes.Friendly}: -{AircraftLostStage2Friend}/{AircraftTotalStage2Friend}");
			sb.Append($"　{BattleRes.Enemy}: -{AircraftLostStage2Enemy}/{AircraftTotalStage2Enemy}");

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
