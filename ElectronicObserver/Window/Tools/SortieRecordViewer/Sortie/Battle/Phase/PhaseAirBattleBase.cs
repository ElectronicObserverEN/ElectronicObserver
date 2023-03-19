using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Properties.Data;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseAirBattleBase : PhaseBase
{
	private ApiKouku AirBattleData { get; }
	private int WaveIndex { get; }

	public List<int> LaunchedShipIndexFriend { get; }
	public List<int> LaunchedShipIndexEnemy { get; }

	private List<AirBattleAttack> Attacks { get; } = new();
	public List<AirBattleAttackViewModel> AttackDisplays { get; } = new();

	public string? Stage1Display => AirBattleData.ApiStage1 switch
	{
		{ } st1 => $"""
		Stage 1: {Constants.GetAirSuperiority(st1.ApiDispSeiku)}
		　{BattleRes.Friendly}: -{st1.ApiFLostcount}/{st1.ApiFCount}
		　{BattleRes.Enemy}: -{st1.ApiELostcount}/{st1.ApiECount}
		""",

		_ => null,
	};

	public string? TouchAircraftFriend => AirBattleData.ApiStage1?.ApiTouchPlane switch
	{
		[EquipmentId id and > 0, ..] => $"　{BattleRes.Contact}: {KCDatabase.Instance.MasterEquipments[(int)id].NameEN}",
		_ => null,
	};

	public string? TouchAircraftEnemy => AirBattleData.ApiStage1?.ApiTouchPlane switch
	{
		[_, EquipmentId id and > 0, ..] => $"　{BattleRes.EnemyContact}: {KCDatabase.Instance.MasterEquipments[(int)id].NameEN}",
		_ => null,
	};

	public string? Stage2Display { get; set; }

	protected PhaseAirBattleBase(ApiKouku airBattleData)
	{
		AirBattleData = airBattleData;

		LaunchedShipIndexFriend = GetLaunchedShipIndex(airBattleData, 0);
		LaunchedShipIndexEnemy = GetLaunchedShipIndex(airBattleData, 1);
	}

	private static List<int> GetLaunchedShipIndex(ApiKouku airBattleData, int index) =>
		airBattleData.ApiPlaneFrom
			.Skip(index)
			.FirstOrDefault()
			?.Where(i => i > 0)
			.Select(i => i - 1)
			.ToList()
		?? new();

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();

		if (AirBattleData.ApiStage2 is { } st2)
		{
			StringBuilder sb = new();

			sb.Append("Stage 2:");
			if (st2.ApiAirFire is not null)
			{
				sb.AppendFormat(BattleRes.AaciType,
					battleFleets.GetShip(new(st2.ApiAirFire.ApiIdx, FleetFlag.Player)),
					AntiAirCutIn.FromId(st2.ApiAirFire.ApiKind).EquipmentConditionsSingleLineDisplay(),
					st2.ApiAirFire.ApiKind);
			}
			sb.AppendLine();
			sb.AppendLine($"　{BattleRes.Friendly}: -{st2.ApiFLostcount}/{st2.ApiFCount}");
			sb.AppendLine($"　{BattleRes.Enemy}: -{st2.ApiELostcount}/{st2.ApiECount}");

			Stage2Display = sb.ToString();
		}

		Attacks.AddRange(GetAttacks(FleetFlag.Player, 0, battleFleets.Fleet,
			AirBattleData.ApiStage3.ApiFraiFlag.Select(i => i ?? 0).ToList(),
			AirBattleData.ApiStage3.ApiFbakFlag.Select(i => i ?? 0).ToList(),
			AirBattleData.ApiStage3.ApiFclFlag,
			AirBattleData.ApiStage3.ApiFdam));

		Attacks.AddRange(GetAttacks(FleetFlag.Player, 6, battleFleets.EscortFleet,
			AirBattleData.ApiStage3Combined.ApiFraiFlag,
			AirBattleData.ApiStage3Combined.ApiFbakFlag,
			AirBattleData.ApiStage3Combined.ApiFclFlag,
			AirBattleData.ApiStage3Combined.ApiFdam));

		Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 0, battleFleets.EnemyFleet,
			AirBattleData.ApiStage3.ApiEraiFlag,
			AirBattleData.ApiStage3.ApiEbakFlag,
			AirBattleData.ApiStage3.ApiEclFlag,
			AirBattleData.ApiStage3.ApiEdam));
		/*
		Attacks.AddRange(GetAttacks(FleetFlag.Enemy, 6, battleFleets.EnemyEscortFleet,
			AirBattleData.ApiStage3Combined.ApiEraiFlag,
			AirBattleData.ApiStage3Combined.ApiEbakFlag,
			AirBattleData.ApiStage3Combined.ApiEclFlag,
			AirBattleData.ApiStage3Combined.ApiEdam));
		*/

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

}
