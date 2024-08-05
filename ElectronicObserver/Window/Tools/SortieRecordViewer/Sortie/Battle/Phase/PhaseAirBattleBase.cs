using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Interfaces;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseAirBattleBase : AttackPhaseBase, IPhaseAirBattle
{
	public override string Title => BattleRes.BattlePhaseAirBattle;

	protected IKCDatabase KcDatabase { get; }

	/// <summary>
	/// <see cref="IApiAirBattle" /> or <see cref="IApiJetAirBattle" />
	/// </summary>
	private object AirBattleData { get; }

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

	private ApiStage1? Stage1 => AirBattleData switch
	{
		IApiAirBattle aab => aab.ApiStage1,
		_ => null,
	};

	[MemberNotNullWhen(true, nameof(Stage2))]
	public bool IsStage2Available => Stage2 is not null;

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

	public int WaveIndex { get; }

	public List<int> LaunchedShipIndexFriend { get; }
	public List<int> LaunchedShipIndexEnemy { get; }

	private List<AirBattleAttack> Attacks { get; } = [];
	public override List<AirBattleAttackViewModel> AttackDisplays { get; } = [];

	public string? Stage1Display { get; }

	public string? TouchAircraftFriend => Stage1?.ApiTouchPlane switch
	{
		[EquipmentId id and > 0, ..] => KcDatabase.MasterEquipments[(int)id]?.NameEN,
		_ => null,
	};
	public string? TouchAircraftFriendDisplay => TouchAircraftFriend switch
	{
		string aircraft => $"　{BattleRes.Contact}: {aircraft}",
		_ => null,
	};

	public string? TouchAircraftEnemy => Stage1?.ApiTouchPlane switch
	{
		[_, EquipmentId id and > 0, ..] => KcDatabase.MasterEquipments[(int)id]?.NameEN,
		_ => null,
	};
	public string? TouchAircraftEnemyDisplay => TouchAircraftEnemy switch
	{
		string aircraft => $"　{BattleRes.EnemyContact}: {aircraft}",
		_ => null,
	};

	public string? Stage2Display { get; private set; }

	protected PhaseAirBattleBase(IKCDatabase kcDatabase, IApiAirBattle airBattleData, int waveIndex = 0)
	{
		KcDatabase = kcDatabase;
		AirBattleData = airBattleData;
		WaveIndex = waveIndex;

		if (airBattleData.ApiStage1 is not null)
		{
			AirState = airBattleData.ApiStage1.ApiDispSeiku;
			AircraftLostStage1Friend = airBattleData.ApiStage1.ApiFLostcount;
			AircraftTotalStage1Friend = airBattleData.ApiStage1.ApiFCount;
			AircraftLostStage1Enemy = airBattleData.ApiStage1.ApiELostcount;
			AircraftTotalStage1Enemy = airBattleData.ApiStage1.ApiECount;

			Stage1Display = GetStage1Display
			(
				AirState,
				AircraftLostStage1Friend,
				AircraftTotalStage1Friend,
				AircraftLostStage1Enemy,
				AircraftTotalStage1Enemy
			);
		}

		LaunchedShipIndexFriend = GetLaunchedShipIndex(airBattleData.ApiPlaneFrom, 0);
		LaunchedShipIndexEnemy = GetLaunchedShipIndex(airBattleData.ApiPlaneFrom, 1);
	}

	protected PhaseAirBattleBase(IKCDatabase kcDatabase, IApiJetAirBattle airBattleData, int waveIndex = 0)
	{
		KcDatabase = kcDatabase;
		AirBattleData = airBattleData;
		WaveIndex = waveIndex;

		if (airBattleData.ApiStage1 is not null)
		{
			AirState = AirState.Unknown;
			AircraftLostStage1Friend = airBattleData.ApiStage1.ApiFLostcount;
			AircraftTotalStage1Friend = airBattleData.ApiStage1.ApiFCount;
			AircraftLostStage1Enemy = airBattleData.ApiStage1.ApiELostcount;
			AircraftTotalStage1Enemy = airBattleData.ApiStage1.ApiECount;

			Stage1Display = GetStage1Display
			(
				AirState,
				AircraftLostStage1Friend,
				AircraftTotalStage1Friend,
				AircraftLostStage1Enemy,
				AircraftTotalStage1Enemy
			);
		}

		LaunchedShipIndexFriend = GetLaunchedShipIndex(airBattleData.ApiPlaneFrom, 0);
		LaunchedShipIndexEnemy = GetLaunchedShipIndex(airBattleData.ApiPlaneFrom, 1);
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
		}
		else if (Stage2Jet is { } stage2Jet)
		{
			AircraftLostStage2Friend = stage2Jet.ApiFLostcount;
			AircraftTotalStage2Friend = stage2Jet.ApiFCount;
			AircraftLostStage2Enemy = stage2Jet.ApiELostcount;
			AircraftTotalStage2Enemy = stage2Jet.ApiECount;
		}

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

		ApiStage3? stage3 = Stage3;
		ApiStage3Jet? stage3Jet = Stage3Jet;
		ApiStage3Combined? stage3Combined = Stage3Combined;
		ApiStage3JetCombined? stage3JetCombined = Stage3JetCombined;

		if (stage3 is not null)
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Player, 0, FleetsAfterPhase.Fleet,
				stage3.ApiFraiFlag.Select(i => i ?? 0).ToList(),
				stage3.ApiFbakFlag.Select(i => i ?? 0).ToList(),
				stage3.ApiFclFlag,
				stage3.ApiFdam));
		}

		if (stage3Jet is not null)
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 0, FleetsAfterPhase.EnemyFleet,
				stage3Jet.ApiEraiFlag,
				stage3Jet.ApiEbakFlag,
				stage3Jet.ApiEclFlag,
				stage3Jet.ApiEdam));
		}

		if (stage3Combined is { ApiFraiFlag: not null, ApiFbakFlag: not null, ApiFclFlag: not null, ApiFdam: not null })
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Player, 6, FleetsAfterPhase.EscortFleet,
				stage3Combined.ApiFraiFlag.Select(i => i ?? 0).ToList(),
				stage3Combined.ApiFbakFlag.Select(i => i ?? 0).ToList(),
				stage3Combined.ApiFclFlag,
				stage3Combined.ApiFdam));
		}

		if (stage3JetCombined is { ApiEraiFlag: not null, ApiEbakFlag: not null, ApiEclFlag: not null, ApiEdam: not null })
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 6, FleetsAfterPhase.EnemyEscortFleet,
				stage3JetCombined.ApiEraiFlag,
				stage3JetCombined.ApiEbakFlag,
				stage3JetCombined.ApiEclFlag,
				stage3JetCombined.ApiEdam));
		}

		if (stage3 is not null)
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 0, FleetsAfterPhase.EnemyFleet,
				stage3.ApiEraiFlag,
				stage3.ApiEbakFlag,
				stage3.ApiEclFlag,
				stage3.ApiEdam));
		}

		if (stage3Combined is { ApiEraiFlag: not null, ApiEbakFlag: not null, ApiEclFlag: not null, ApiEdam: not null })
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 6, FleetsAfterPhase.EnemyEscortFleet,
				stage3Combined.ApiEraiFlag,
				stage3Combined.ApiEbakFlag,
				stage3Combined.ApiEclFlag,
				stage3Combined.ApiEdam));
		}

		foreach (AirBattleAttack attack in Attacks.Where(attack => attack.AttackType is not AirAttack.None))
		{
			AttackDisplays.Add(new(FleetsAfterPhase, WaveIndex, attack));
			AddDamage(FleetsAfterPhase, attack.Defenders.First().Defender, attack.Defenders.First().Damage);
		}

		return FleetsAfterPhase.Clone();
	}
}
