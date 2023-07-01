using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Properties.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseFriendlyShelling : PhaseBase
{
	public override string Title => BattleRes.BattlePhaseFriendlyShelling;

	private ApiFriendlyBattle ApiFriendlyBattle { get; }
	private ApiHougeki ShellingData => ApiFriendlyBattle.ApiHougeki;

	private int FlareIndexFriend { get; set; }
	private int FlareIndexEnemy { get; set; }
	private IShipData? FlareFriend { get; set; }
	private IShipData? FlareEnemy { get; set; }

	private int SearchlightIndexFriend { get; set; }
	private int SearchlightIndexEnemy { get; set; }
	private IShipData? SearchlightFriend { get; set; }
	private IShipData? SearchlightEnemy { get; set; }

	public string InitialDisplay => GetDisplay();

	private List<PhaseNightBattleAttack> Attacks { get; } = new();
	public List<PhaseFriendNightBattleAttackViewModel> AttackDisplays { get; } = new();

	public PhaseFriendlyShelling(ApiFriendlyBattle apiFriendlyBattle)
	{
		static int ParseInt(object e) => e switch
		{
			JsonElement { ValueKind: JsonValueKind.Number } n => n.GetInt32(),
			JsonElement { ValueKind: JsonValueKind.String } s => int.Parse(s.GetString()!),
			_ => throw new NotImplementedException(),
		};

		ApiFriendlyBattle = apiFriendlyBattle;

		// assumption here that friend night battle can never be like sub vs sub night battle
		List<FleetFlag> fleetflag = ShellingData.ApiAtEflag!;
		List<int> attackers = ShellingData.ApiAtList!;
		List<int> nightAirAttackFlags = ShellingData.ApiNMotherList!;
		List<NightAttackKind> attackTypes = ShellingData.ApiSpList!;
		List<List<int>> defenders = ShellingData.ApiDfList!.Select(elem => elem.Where(e => e != -1).ToList()).ToList();
		List<List<int>> attackEquipments = ShellingData.ApiSiList!.Select(elem => elem.Select(ParseInt).ToList()).ToList();
		List<List<HitType>> criticals = ShellingData.ApiClList!.Select(elem => elem.Where(e => e != HitType.Invalid).ToList()).ToList();
		List<List<double>> rawDamages = ShellingData.ApiDamage!.Select(elem => elem.Where(e => e != -1).ToList()).ToList();

		for (int i = 0; i < attackers.Count; i++)
		{
			PhaseNightBattleAttack attack = new()
			{
				Attacker = new BattleIndex(attackers[i], fleetflag[i]),
				NightAirAttackFlag = nightAirAttackFlags[i] == -1,
				AttackType = attackTypes[i],
				EquipmentIDs = attackEquipments[i],
			};

			static FleetFlag DefenderFlag(FleetFlag flag) => flag switch
			{
				FleetFlag.Player => FleetFlag.Enemy,
				_ => FleetFlag.Player,
			};

			for (int k = 0; k < defenders[i].Count; k++)
			{
				PhaseNightBattleDefender defender = new()
				{
					Defender = new BattleIndex(defenders[i][k], DefenderFlag(fleetflag[i])),
					CriticalFlag = criticals[i][k],
					RawDamage = rawDamages[i][k],
				};

				attack.Defenders.Add(defender);
			}

			Attacks.Add(attack);
		}
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();

		// this part is mostly the same as PhaseNightInitial
		// might be a good idea to refactor to avoid duplication at some point?
		FlareIndexFriend = ApiFriendlyBattle.ApiFlarePos[0];
		FlareIndexEnemy = ApiFriendlyBattle.ApiFlarePos[1];
		FlareFriend = GetFlareFriend(battleFleets, ApiFriendlyBattle.ApiFlarePos[0]);
		FlareEnemy = GetFlareEnemy(battleFleets, false, ApiFriendlyBattle.ApiFlarePos[1]);

		(SearchlightFriend, SearchlightIndexFriend) = GetSearchlightShip(battleFleets.FriendFleet);
		(SearchlightEnemy, SearchlightIndexEnemy) = GetSearchlightShip(battleFleets.EnemyFleet);

		foreach (PhaseNightBattleAttack atk in Attacks)
		{
			foreach (IGrouping<BattleIndex, PhaseNightBattleDefender> defs in atk.Defenders.GroupBy(d => d.Defender))
			{
				AttackDisplays.Add(new PhaseFriendNightBattleAttackViewModel(battleFleets, atk.Attacker, defs.Key, atk.AttackType, defs.ToList()));
				AddFriendDamage(battleFleets, defs.Key, defs.Sum(d => d.Damage));
			}
		}

		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}

	private static (IShipData? Ship, int Index) GetSearchlightShip(IFleetData? fleet) => fleet switch
	{
		null => (null, -1),

		_ => fleet.MembersWithoutEscaped?
			.Select((s, i) => (Ship: s, Index: i))
			.Where(t => t.Ship?.HPCurrent > 1)
			.FirstOrDefault(t => t.Ship!.HasSearchlight()) ?? (null, -1),
	};

	private IShipData? GetFlareFriend(BattleFleets fleets, int index) => index switch
	{
		> 0 => fleets.FriendFleet!.MembersInstance[index],
		_ => null,
	};

	private IShipData? GetFlareEnemy(BattleFleets fleets, bool isEscort, int index) => (isEscort, index) switch
	{
		(false, > 0) => fleets.EnemyFleet?.MembersInstance[index],
		(true, > 0) => fleets.EnemyEscortFleet?.MembersInstance[index - 6],
		_ => null,
	};

	private string GetDisplay()
	{
		List<string> values = new();

		if (SearchlightFriend is not null)
		{
			values.Add(string.Format(ConstantsRes.BattleDetail_FriendlySearchlight, SearchlightFriend.NameWithLevel, SearchlightIndexFriend + 1));
		}

		if (SearchlightEnemy is not null)
		{
			values.Add(string.Format(ConstantsRes.BattleDetail_EnemySearchlight, SearchlightEnemy.NameWithLevel, SearchlightIndexEnemy + 1));
		}

		if (FlareFriend is not null)
		{
			values.Add(string.Format(ConstantsRes.BattleDetail_FriendlyStarshell, FlareFriend.NameWithLevel, FlareIndexFriend + 1));
		}

		if (FlareEnemy is not null)
		{
			values.Add(string.Format(ConstantsRes.BattleDetail_EnemyStarshell, FlareEnemy.NameWithLevel, FlareIndexEnemy + 1));
		}

		return string.Join("\n", values);
	}
}
