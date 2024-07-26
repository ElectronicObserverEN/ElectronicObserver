using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseNightInitial : PhaseBase
{
	public override string Title => BattleRes.NightBattleBegins;

	private IKCDatabase KcDatabase { get; }
	private BattleFleets Fleets { get; }

	private bool IsEscort { get; }
	private bool IsEnemyEscort { get; }

	public IEquipmentDataMaster? TouchAircraftFriend { get; }
	public IEquipmentDataMaster? TouchAircraftEnemy { get; }

	public int FlareIndexFriend { get; }
	public int FlareIndexEnemy { get; }
	public IShipData? FlareFriend { get; }
	public IShipData? FlareEnemy { get; }

	public int SearchlightIndexFriend { get; }
	public int SearchlightIndexEnemy { get; }
	public IShipData? SearchlightFriend { get; }
	public IShipData? SearchlightEnemy { get; }
	public IEquipmentData? SearchlightEquipmentFriend => GetSearchlight(SearchlightFriend);
	public IEquipmentData? SearchlightEquipmentEnemy => GetSearchlight(SearchlightEnemy);

	public string Display => GetDisplay();

	public PhaseNightInitial(IKCDatabase kcDatabase, BattleFleets fleets, INightGearApiResponse battle)
	{
		KcDatabase = kcDatabase;
		Fleets = fleets;

		TouchAircraftFriend = GetTouchAircraft(battle.ApiTouchPlane[0]);
		TouchAircraftEnemy = GetTouchAircraft(battle.ApiTouchPlane[1]);

		FlareIndexFriend = battle.ApiFlarePos[0];
		FlareIndexEnemy = battle.ApiFlarePos[1];
		FlareFriend = GetFlareFriend(fleets.EscortFleet is not null, battle.ApiFlarePos[0]);
		FlareEnemy = GetFlareEnemy(fleets.EnemyEscortFleet is not null, battle.ApiFlarePos[1]);

		(SearchlightFriend, SearchlightIndexFriend) = GetSearchlightShip(fleets.EscortFleet ?? fleets.Fleet);
		(SearchlightEnemy, SearchlightIndexEnemy) = GetSearchlightShip(NightBattleEnemyFleet(fleets));

		if (SearchlightIndexFriend >= 0 && fleets.EscortFleet is not null)
		{
			SearchlightIndexFriend += 6;
		}

		if (SearchlightIndexEnemy >= 0 && fleets.EnemyEscortFleet is not null)
		{
			SearchlightIndexEnemy += 6;
		}
	}

	public PhaseNightInitial(IKCDatabase kcDatabase, BattleFleets fleets, ICombinedNightBattleApiResponse battle)
	{
		KcDatabase = kcDatabase;
		Fleets = fleets;

		IsEscort = battle.ApiActiveDeck[0] != 1;
		IsEnemyEscort = battle.ApiActiveDeck[1] != 1;

		TouchAircraftFriend = GetTouchAircraft(battle.ApiTouchPlane[0]);
		TouchAircraftEnemy = GetTouchAircraft(battle.ApiTouchPlane[1]);

		FlareIndexFriend = battle.ApiFlarePos[0];
		FlareIndexEnemy = battle.ApiFlarePos[1];
		FlareFriend = GetFlareFriend(IsEscort, battle.ApiFlarePos[0]);
		FlareEnemy = GetFlareEnemy(IsEnemyEscort, battle.ApiFlarePos[1]);

		(SearchlightFriend, SearchlightIndexFriend) = GetSearchlightShip(fleets.EscortFleet);
		(SearchlightEnemy, SearchlightIndexEnemy) = GetSearchlightShip(NightBattleEnemyFleet(fleets));

		if (SearchlightIndexFriend >= 0)
		{
			SearchlightIndexFriend += 6;
		}

		if (SearchlightIndexEnemy >= 0)
		{
			SearchlightIndexEnemy += 6;
		}
	}

	private static IFleetData? NightBattleEnemyFleet(BattleFleets fleets)
	{
		// todo: test case 5-3 (night battle only): EnemyFleet is null
		// I'm not sure why it's null but I think it shouldn't be
		if (fleets.EnemyFleet is null) return null;
		if (fleets.EnemyEscortFleet is null) return fleets.EnemyFleet;

		int score = 0;

		foreach ((IShipData? ship, int i) in fleets.EnemyEscortFleet.MembersInstance.Select((s, i) => (s, i)))
		{
			if (ship is null) continue;

			if (i == 0 && ship.HPRate > 0)
			{
				score += 10;
			}

			score += ship.HPRate switch
			{
				> 0.5 => 10,
				> 0.25 => 7,
				_ => 0,
			};
		}

		return score switch
		{
			< 30 => fleets.EnemyFleet,
			_ => fleets.EnemyEscortFleet,
		};
	}

	private IEquipmentDataMaster? GetTouchAircraft(object value)
	{
		int equipmentId = value switch
		{
			JsonElement { ValueKind: JsonValueKind.String } s => int.Parse(s.GetString()!),
			JsonElement { ValueKind: JsonValueKind.Number } s => s.GetInt32(),
			_ => -1,
		};

		return equipmentId switch
		{
			> 0 => KcDatabase.MasterEquipments[equipmentId],
			_ => null,
		};
	}

	private IEquipmentData? GetSearchlight(IShipData? ship) => ship?.AllSlotInstance
		.FirstOrDefault(e => e?.MasterEquipment.CategoryType is EquipmentTypes.SearchlightLarge)
		?? ship?.AllSlotInstance
			.FirstOrDefault(e => e?.MasterEquipment.CategoryType is EquipmentTypes.Searchlight);

	private static (IShipData? Ship, int Index) GetSearchlightShip(IFleetData? fleet)
	{
		if (fleet is null) return (null, -1);

		List<(IShipData? Ship, int Index)>? searchlightCandidates = fleet.MembersWithoutEscaped?
			.Select((s, i) => (Ship: s, Index: i))
			.Where(t => t.Ship?.HPCurrent > 1)
			.ToList()!;

		if (searchlightCandidates is null) return (null, -1);

		return searchlightCandidates.FirstOrDefault(t => t.Ship!.HasLargeSearchlight(),
			searchlightCandidates.FirstOrDefault(t => t.Ship!.HasSearchlight(), (null, -1)));
	}

	private IShipData? GetFlareFriend(bool isEscort, int index) => (isEscort, index) switch
	{
		(false, > 0) => Fleets.Fleet.MembersInstance[index],
		(true, > 0) => Fleets.EscortFleet?.MembersInstance[index - 6],
		_ => null,
	};

	private IShipData? GetFlareEnemy(bool isEscort, int index) => (isEscort, index) switch
	{
		(false, > 0) => Fleets.EnemyFleet?.MembersInstance[index],
		(true, > 0) => Fleets.EnemyEscortFleet?.MembersInstance[index - 6],
		_ => null,
	};

	private string GetDisplay()
	{
		List<string> values = new();

		if (TouchAircraftFriend is not null)
		{
			values.Add(ConstantsRes.BattleDetail_FriendlyNightContact + TouchAircraftFriend.NameEN);
		}

		if (TouchAircraftEnemy is not null)
		{
			values.Add(ConstantsRes.BattleDetail_EnemyNightContact + TouchAircraftEnemy.NameEN);
		}

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
