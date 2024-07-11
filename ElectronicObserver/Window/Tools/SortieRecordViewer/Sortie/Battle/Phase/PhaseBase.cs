using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public abstract class PhaseBase
{
	public abstract string Title { get; }

	public BattleFleets? FleetsBeforePhase { get; protected set; }
	public BattleFleets? FleetsAfterPhase { get; protected set; }

	public virtual BattleFleets EmulateBattle(BattleFleets battleFleets, List<int> damages)
	{
		FleetsBeforePhase = battleFleets.Clone();
		FleetsAfterPhase = battleFleets;

		return FleetsAfterPhase.Clone();
	}

	protected static void AddDamage(BattleFleets fleets, BattleIndex index, int damage)
	{
		IShipData? ship = fleets.GetShip(index);

		if (ship is not ShipDataMock s) return;

		s.HPCurrent -= damage;

		if (s.HPCurrent > 0) return;

		IEquipmentData? damecon = s.AllSlotInstance
			.FirstOrDefault(e => e?.MasterEquipment.CategoryType is EquipmentTypes.DamageControl);

		if (damecon is null) return;

		s.HPCurrent = damecon switch
		{
			{ EquipmentId: EquipmentId.DamageControl_EmergencyRepairGoddess } => s.HPMax,
			{ EquipmentId: EquipmentId.DamageControl_EmergencyRepairPersonnel } => (int)(ship.HPMax * 0.2),
			_ => s.HPCurrent,
		};

		if (s.ExpansionSlotInstance == damecon)
		{
			s.ExpansionSlotInstance = null;
		}

		if (s.SlotInstance.IndexOf(damecon) is int i and >= 0)
		{
			s.SlotInstance[i] = null;
		}
	}

	protected static void AddFriendDamage(BattleFleets fleets, BattleIndex index, int damage)
	{
		IShipData? ship = fleets.GetFriendShip(index);

		if (ship is ShipDataMock s)
		{
			s.HPCurrent -= damage;
		}
	}

	protected static void AddAirBaseDamage(BattleFleets fleets, BattleIndex index, int damage)
	{
		IBaseAirCorpsData? airBase = fleets.GetAirBase(index);

		if (airBase is BaseAirCorpsDataMock ab)
		{
			ab.HPCurrent -= damage;
		}
	}

	protected static string GetStage1Display(AirState airState, int friendlyLost, int friendlyTotal,
		int enemyLost, int enemyTotal) =>
		$"""
		Stage 1: {Constants.GetAirSuperiority(airState)}
		　{BattleRes.Friendly}: -{friendlyLost}/{friendlyTotal}
		　{BattleRes.Enemy}: -{enemyLost}/{enemyTotal}
		""";

	protected static List<AirBattleAttack> GetAttacks(FleetFlag fleetFlag, int indexOffset, IFleetData? fleet,
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
							// 0.1 is used for flag protect miss
							(AirHitType.HitOrMiss, < 1) => HitType.Miss,
							(AirHitType.HitOrMiss, _) => HitType.Hit,

							_ => HitType.Invalid,
						},
						RawDamage = t.Damage,
					},
				},
			}).ToList()
			?? new();

	protected static List<int> GetLaunchedShipIndex(List<List<int>?> apiPlaneFrom, int index)
		=> apiPlaneFrom
			.Skip(index)
			.FirstOrDefault()
			?.Where(i => i > 0)
			.Select(i => i - 1)
			.ToList()
			?? new();
}
