using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Properties.Data;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseBaseAirRaid : PhaseBase
{
	public override string Title => BattleRes.BattlePhaseAirBaseRaid;

	private ApiAirBaseRaid AirBattleData { get; }

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
		[EquipmentId id and > 0, ..] => $"　{BattleRes.Contact}: {KCDatabase.Instance.MasterEquipments[(int)id].NameEN}",
		_ => null,
	};

	public string? TouchAircraftEnemy => Stage1?.ApiTouchPlane switch
	{
		[_, EquipmentId id and > 0, ..] => $"　{BattleRes.EnemyContact}: {KCDatabase.Instance.MasterEquipments[(int)id].NameEN}",
		_ => null,
	};

	public string? Stage2Display { get; protected set; }

	private List<BattleBaseAirCorpsSquadron> Squadrons { get; }

	public string Display { get; }

	public PhaseBaseAirRaid(ApiAirBaseRaid airBattleData, int waveIndex = 0)
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

		if (airBattleData.ApiMapSquadronPlane is null)
		{

		}

		Squadrons = airBattleData.ApiMapSquadronPlane?.Values
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

		ApiStage3? stage3 = Stage3;

		if (stage3 is not null)
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Player, 0, battleFleets.Fleet,
				stage3.ApiFraiFlag.Select(i => i ?? 0).ToList(),
				stage3.ApiFbakFlag.Select(i => i ?? 0).ToList(),
				stage3.ApiFclFlag,
				stage3.ApiFdam));
		}

		if (stage3 is not null)
		{
			Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 0, battleFleets.EnemyFleet,
				stage3.ApiEraiFlag,
				stage3.ApiEbakFlag,
				stage3.ApiEclFlag,
				stage3.ApiEdam));
		}

		foreach (AirBattleAttack attack in Attacks.Where(attack => attack.AttackType is not AirAttack.None))
		{
			AttackDisplays.Add(new(battleFleets, WaveIndex, attack));
			AddAirBaseDamage(battleFleets, attack.Defenders.First().Defender, attack.Defenders.First().Damage);
		}

		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}

	private string GetStage1Display(AirState airState, int friendlyLost, int friendlyTotal,
		int enemyLost, int enemyTotal) =>
		$"""
		Stage 1: {Constants.GetAirSuperiority(airState)}
		　{BattleRes.Friendly}: -{friendlyLost}/{friendlyTotal}
		　{BattleRes.Enemy}: -{enemyLost}/{enemyTotal}
		""";

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

	private static List<int> GetLaunchedShipIndex(List<List<int>?> apiPlaneFrom, int index) =>
		apiPlaneFrom
			.Skip(index)
			.FirstOrDefault()
			?.Where(i => i > 0)
			.Select(i => i - 1)
			.ToList()
		?? new();
}
