using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseAirBattleBase : PhaseBase
{
	public override string Title => BattleRes.BattlePhaseAirBattle;

	protected IKCDatabase KcDatabase { get; }

	/// <summary>
	/// <see cref="IApiAirBattle" /> or <see cref="IApiJetAirBattle" />
	/// </summary>
	private object AirBattleData { get; }

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

	public int WaveIndex { get; }

	public List<int> LaunchedShipIndexFriend { get; }
	public List<int> LaunchedShipIndexEnemy { get; }

	private List<AirBattleAttack> Attacks { get; } = new();
	public List<AirBattleAttackViewModel> AttackDisplays { get; } = new();

	public string? Stage1Display { get; }

	public string? TouchAircraftFriend => Stage1?.ApiTouchPlane switch
	{
		[EquipmentId id and > 0, ..] => KcDatabase.MasterEquipments[(int)id].NameEN,
		_ => null,
	};
	public string? TouchAircraftFriendDisplay => TouchAircraftFriend switch
	{
		string aircraft => $"　{BattleRes.Contact}: {aircraft}",
		_ => null,
	};

	public string? TouchAircraftEnemy => Stage1?.ApiTouchPlane switch
	{
		[_, EquipmentId id and > 0, ..] => KcDatabase.MasterEquipments[(int)id].NameEN,
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
			Stage1FLostcount = airBattleData.ApiStage1.ApiFLostcount;
			Stage1FCount = airBattleData.ApiStage1.ApiFCount;
			Stage1ELostcount = airBattleData.ApiStage1.ApiELostcount;
			Stage1ECount = airBattleData.ApiStage1.ApiECount;

			Stage1Display = GetStage1Display
			(
				AirState,
				Stage1FLostcount,
				Stage1FCount,
				Stage1ELostcount,
				Stage1ECount
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
			AirState = AirState.Unknown;
			Stage1FLostcount = airBattleData.ApiStage1.ApiFLostcount;
			Stage1FCount = airBattleData.ApiStage1.ApiFCount;
			Stage1ELostcount = airBattleData.ApiStage1.ApiELostcount;
			Stage1ECount = airBattleData.ApiStage1.ApiECount;

			Stage1Display = GetStage1Display
			(
				AirState,
				Stage1FLostcount,
				Stage1FCount,
				Stage1ELostcount,
				Stage1ECount
			);
		}

		LaunchedShipIndexFriend = GetLaunchedShipIndex(airBattleData.ApiPlaneFrom, 0);
		LaunchedShipIndexEnemy = GetLaunchedShipIndex(airBattleData.ApiPlaneFrom, 1);
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
		}
		else if (Stage2Jet is { } stage2Jet)
		{
			Stage2FLostcount = stage2Jet.ApiFLostcount;
			Stage2FCount = stage2Jet.ApiFCount;
			Stage2ELostcount = stage2Jet.ApiELostcount;
			Stage2ECount = stage2Jet.ApiECount;
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
		sb.AppendLine($"　{BattleRes.Friendly}: -{Stage2FLostcount}/{Stage2FCount}");
		sb.Append($"　{BattleRes.Enemy}: -{Stage2ELostcount}/{Stage2ECount}");

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
