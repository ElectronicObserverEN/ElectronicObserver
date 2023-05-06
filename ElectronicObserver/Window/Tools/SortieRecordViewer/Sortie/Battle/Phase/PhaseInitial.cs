using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Properties.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Tools.FleetImageGenerator;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseInitial : PhaseBase
{
	private IKCDatabase KcDatabase { get; }

	private List<int> FriendInitialHPs { get; }

	private List<IShipData?> EnemyMembersInstance { get; }
	private List<IShipData?>? EnemyMembersEscortInstance { get; }

	public string Title => BattleRes.Participant;

	private string PlayerMainFleetTitle => FleetsAfterPhase?.EscortFleet switch
	{
		null => ConstantsRes.BattleDetail_FriendFleet,
		_ => ConstantsRes.BattleDetail_FriendMainFleet,
	};

	private string EnemyMainFleetTitle => FleetsAfterPhase?.EnemyEscortFleet switch
	{
		null => ConstantsRes.BattleDetail_EnemyFleet,
		_ => ConstantsRes.BattleDetail_EnemyMainFleet,
	};

	public string? PlayerMainFleetDisplay => FleetDisplay(FleetsAfterPhase?.Fleet, false, PlayerMainFleetTitle);
	public string? PlayerEscortFleetDisplay => FleetDisplay(FleetsAfterPhase?.EscortFleet, true, ConstantsRes.BattleDetail_FriendEscortFleet);
	public string? EnemyMainFleetDisplay => EnemyFleetDisplay(FleetsAfterPhase?.EnemyFleet, false, EnemyMainFleetTitle);
	public string? EnemyEscortFleetDisplay => EnemyFleetDisplay(FleetsAfterPhase?.EnemyEscortFleet, true, ConstantsRes.BattleDetail_EnemyEscortFleet);

	public string? AirBaseDisplay => HasAirBaseAttack switch
	{
		true => MakeAirBaseString(),
		_ => null,
	};

	private static int GetFleetIndex(int index, bool isEscort) => isEscort switch
	{
		true => index + 7,
		_ => index + 1,
	};

	private static string? FleetDisplay(IFleetData? fleet, bool isEscort, string title) => fleet switch
	{
		null => null,
		_ => $"{FleetHeader(fleet, title)}\n" +
			string.Join("\n", fleet.MembersInstance.Select((ship, i) =>
				ShipDisplay(fleet, ship, GetFleetIndex(i, isEscort)))),
	};

	private static string FleetHeader(IFleetData fleet, string fleetTitle) =>
		$"{fleetTitle} " +
		$"{BattleRes.AirSuperiority} {Calculator.GetAirSuperiorityRangeString(fleet)} " +
		$"{GetLosString(fleet)}";

	private static string? ShipDisplay(IFleetData fleet, IShipData? ship, int index) => ship switch
	{
		null => null,

		_ => $"#{index}: {ship.MasterShip.ShipTypeName} {ship.NameWithLevel} " +
			$"HP: {ship.HPCurrent}/{ship.HPMax} - " +
			$"{GeneralRes.Firepower}:{ship.FirepowerBase}, " +
			$"{GeneralRes.Torpedo}:{ship.TorpedoBase}, " +
			$"{GeneralRes.AntiAir}:{ship.AABase}, " +
			$"{GeneralRes.Armor}:{ship.ArmorBase}" +
			$"{EscapedText(fleet.EscapedShipList.Contains(ship.MasterID))}" +
			$"\n" +
			$" {string.Join(", ", ship.AllSlotInstance.Zip(ship.ExpansionSlot switch
			{
				> 0 => ship.Aircraft.Concat(new[] { 0 }),
				_ => ship.Aircraft,
			},
				(eq, aircraft) => eq switch
				{
					null => null,
					{ MasterEquipment.IsAircraft: true } => $"[{aircraft}] {eq.NameWithLevel}",
					_ => eq.NameWithLevel,
				}
			).Where(str => str != null))}",
	};

	private static string EscapedText(bool isEscaped) => isEscaped switch
	{
		true => $" ({BattleRes.Escaped})",
		_ => "",
	};

	private static string? EnemyFleetDisplay(IFleetData? fleet, bool isEscort, string title) => fleet switch
	{
		null => null,
		_ => $"{EnemyFleetHeader(fleet, title)}\n" +
			string.Join("\n", fleet.MembersInstance.Select((ship, i) =>
				EnemyShipDisplay(ship, GetFleetIndex(i, isEscort)))),
	};

	private static string EnemyFleetHeader(IFleetData fleet, string fleetTitle) =>
		$"{fleetTitle} " +
		$"{string.Format(BattleRes.AirBaseAirPower, Calculator.GetAirSuperiority(fleet), AirBaseAirPower(fleet))}";

	private static int AirBaseAirPower(IFleetData fleet) => fleet.MembersInstance
		.Where(s => s is not null)
		.Sum(s => s!.AllSlotInstance.Zip(s.Aircraft, (eq, size) => (Equipment: eq, Size: size))
			.Where(slot => slot.Equipment?.MasterEquipment.IsAircraft ?? false)
			.Sum(slot => Calculator.GetAirSuperiority(slot.Equipment!.EquipmentID, slot.Size, 0, 0, AirBaseActionKind.Mission)));

	private static string? EnemyShipDisplay(IShipData? ship, int index) => ship switch
	{
		null => null,

		_ => $"#{index}: ID:{ship.MasterShip.ShipID} {ship.MasterShip.ShipTypeName} {ship.NameWithLevel} " +
			$"HP: {ship.HPCurrent}/{ship.HPMax} - " +
			$"{GeneralRes.Firepower}:{ship.FirepowerBase}, " +
			$"{GeneralRes.Torpedo}:{ship.TorpedoBase}, " +
			$"{GeneralRes.AntiAir}:{ship.AABase}, " +
			$"{GeneralRes.Armor}:{ship.ArmorBase}" +
			$"\n" +
			$" {string.Join(", ", ship.AllSlotInstance.Zip(ship.ExpansionSlot switch
			{
				> 0 => ship.Aircraft.Concat(new[] { 0 }),
				_ => ship.Aircraft,
			},
				(eq, aircraft) => eq switch
				{
					null => null,
					{ MasterEquipment.IsAircraft: true } => $"ID:{eq.MasterEquipment.EquipmentID} [{aircraft}] {eq.NameWithLevel}",
					_ => $"ID:{eq.MasterEquipment.EquipmentID} {eq.NameWithLevel}",
				}
			).Where(str => str != null))}",
	};

	public bool HasAirBaseAttack { get; }

	public PhaseInitial(IKCDatabase kcDatabase, BattleFleets fleets, IBattleApiResponse battle)
	{
		KcDatabase = kcDatabase;
		FleetsBeforePhase = fleets;

		HasAirBaseAttack = battle switch
		{
			IAirBaseBattle abb => abb.ApiAirBaseAttack is not null,
			_ => false,
		};

		FriendInitialHPs = battle.ApiFNowhps;

		EnemyMembersInstance = battle.ApiShipKe
			.Zip(battle.ApiShipLv, (id, level) => (Id: id, Level: level))
			.Zip(battle.ApiESlot, (t, equipment) => (t.Id, t.Level, Equipment: equipment))
			.Zip(battle.ApiENowhps, (t, hp) => (t.Id, t.Level, t.Equipment, Hp: hp))
			.Select(t => t.Id switch
			{
				> 0 => new ShipDataMock(KcDatabase.MasterShips[t.Id])
				{
					HPCurrent = t.Hp switch
					{
						JsonElement { ValueKind: JsonValueKind.Number } n => n.GetInt32(),
						JsonElement { ValueKind: JsonValueKind.String } s => KcDatabase.MasterShips[t.Id].HPMin,
						_ => throw new NotImplementedException(),
					},
					Level = t.Level,
					SlotInstance = t.Equipment
						.Select(id => id switch
						{
							> 0 => new EquipmentDataMock(KcDatabase.MasterEquipments[id]),
							_ => null,
						})
						.Cast<IEquipmentData?>()
						.ToList(),
				},
				_ => null,
			})
			.Cast<IShipData?>()
			.ToList();
	}

	public PhaseInitial(IKCDatabase kcDatabase, BattleFleets fleets, IEnemyCombinedFleetBattle battle)
		: this(kcDatabase, fleets, (IBattleApiResponse)battle)
	{
		EnemyMembersEscortInstance = MakeEnemyEscortFleet(battle);
	}

	public PhaseInitial(IKCDatabase kcDatabase, BattleFleets fleets, ICombinedBattleApiResponse battle)
		: this(kcDatabase, fleets, (IBattleApiResponse)battle)
	{
		EnemyMembersEscortInstance = MakeEnemyEscortFleet(battle);
	}

	private List<IShipData?>? MakeEnemyEscortFleet(IEnemyCombinedFleetBattle battle) => battle.ApiShipKeCombined
		.Zip(battle.ApiShipLvCombined, (id, level) => (Id: id, Level: level))
		.Zip(battle.ApiESlotCombined, (t, equipment) => (t.Id, t.Level, Equipment: equipment))
		.Zip(battle.ApiENowhpsCombined, (t, hp) => (t.Id, t.Level, t.Equipment, Hp: hp))
		.Select(t => t.Id switch
		{
			> 0 => new ShipDataMock(KcDatabase.MasterShips[t.Id])
			{
				HPCurrent = t.Hp,
				Level = t.Level,
				SlotInstance = t.Equipment
					.Select(id => id switch
					{
						> 0 => new EquipmentDataMock(KcDatabase.MasterEquipments[id]),
						_ => null,
					})
					.Cast<IEquipmentData?>()
					.ToList(),
			},
			_ => null,
		})
		.Cast<IShipData?>()
		.ToList();

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		battleFleets = FleetsBeforePhase!.Clone();

		battleFleets.EnemyFleet = new FleetDataMock
		{
			MembersInstance = new(EnemyMembersInstance),
		};

		if (EnemyMembersEscortInstance is not null)
		{
			battleFleets.EnemyEscortFleet = new FleetDataMock
			{
				MembersInstance = new(EnemyMembersEscortInstance),
			};
		}

		foreach ((IShipData? ship, int hp) in battleFleets.Fleet.MembersInstance.Zip(FriendInitialHPs))
		{
			((ShipDataMock)ship).HPCurrent = hp;
		}

		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}

	private static string GetLosString(IFleetData fleet)
	{
		List<LosValue> los = GetLosValues(fleet);

		return string.Format(BattleRes.Los, los[0].Value, los[1].Value, los[2].Value, los[3].Value);
	}

	private static List<LosValue> GetLosValues(IFleetData fleet) => Enumerable.Range(1, 4)
		.Select(w => new LosValue(w, Math.Round(Calculator.GetSearchingAbility_New33(fleet, w), 2, MidpointRounding.ToNegativeInfinity)))
		.ToList();

	private string MakeAirBaseString()
	{
		StringBuilder sb = new();

		sb.AppendLine(ConstantsRes.BattleDetail_AirBase);

		foreach (IBaseAirCorpsData corps in FleetsBeforePhase!.AirBases)
		{
			sb.AppendFormat("{0} [{1}] " + BattleRes.AirSuperiority + " {2}\r\n　{3}\r\n",
				corps.Name, Constants.GetBaseAirCorpsActionKind(corps.ActionKind),
				Calculator.GetAirSuperiorityRangeString(corps),
				string.Join(", ", corps.Squadrons.Values
					.Where(sq => sq is { State: 1, EquipmentInstance: not null })
					.Select(sq => sq.EquipmentInstance!.NameWithLevel)));
		}

		return sb.ToString();
	}
}
