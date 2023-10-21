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
	private IShipData? FlareFriend { get; }
	private IShipData? FlareEnemy { get; }

	private int SearchlightIndexFriend { get; }
	private int SearchlightIndexEnemy { get; }
	private IShipData? SearchlightFriend { get; }
	private IShipData? SearchlightEnemy { get; }

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
		(SearchlightEnemy, SearchlightIndexEnemy) = GetSearchlightShip(fleets.EnemyEscortFleet ?? fleets.EnemyFleet);

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
		(SearchlightEnemy, SearchlightIndexEnemy) = GetSearchlightShip(fleets.EnemyEscortFleet);

		if (SearchlightIndexFriend >= 0)
		{
			SearchlightIndexFriend += 6;
		}

		if (SearchlightIndexEnemy >= 0)
		{
			SearchlightIndexEnemy += 6;
		}
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

	private static (IShipData? Ship, int Index) GetSearchlightShip(IFleetData? fleet) => fleet switch
	{
		null => (null, -1),

		_ => fleet.MembersWithoutEscaped?
			.Select((s, i) => (Ship: s, Index: i))
			.Where(t => t.Ship?.HPCurrent > 1)
			.FirstOrDefault(t => t.Ship!.HasSearchlight()) ?? (null, -1),
	};

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
			values.Add(ConstantsRes.BattleDetail_EnemyNightContact+TouchAircraftEnemy.NameEN);
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
