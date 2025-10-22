using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Notifier;

/// <summary>
/// 大破進撃警告通知を扱います。
/// </summary>
public class NotifierDamage : NotifierBase
{

	/// <summary>
	/// 事前通知(出撃前、戦闘結果判明直後)が有効かどうか
	/// </summary>
	public bool NotifiesBefore { get; set; }

	/// <summary>
	/// 事中通知(出撃前、戦績画面)が有効かどうか
	/// </summary>
	public bool NotifiesNow { get; set; }

	/// <summary>
	/// 事後通知(出撃直後、進撃中)が有効かどうか
	/// </summary>
	public bool NotifiesAfter { get; set; }

	/// <summary>
	/// 通知が有効な艦船のLv下限
	/// これよりLvが低い艦は除外されます
	/// </summary>
	public int LevelBorder { get; set; }

	/// <summary>
	/// 非ロック艦も含める
	/// </summary>
	public bool ContainsNotLockedShip { get; set; }

	/// <summary>
	/// ダメコン装備艦も含める
	/// </summary>
	public bool ContainsSafeShip { get; set; }

	/// <summary>
	/// 旗艦を含める
	/// </summary>
	public bool ContainsFlagship { get; set; }

	/// <summary>
	/// 終点でも通知する
	/// </summary>
	public bool NotifiesAtEndpoint { get; set; }


	public NotifierDamage(Utility.Configuration.ConfigurationData.ConfigNotifierDamage config)
		: base(config)
	{
		DialogData.Title = NotifierRes.DamagedTitle;

		SubscribeToApis();

		NotifiesBefore = config.NotifiesBefore;
		NotifiesNow = config.NotifiesNow;
		NotifiesAfter = config.NotifiesAfter;
		LevelBorder = config.LevelBorder;
		ContainsNotLockedShip = config.ContainsNotLockedShip;
		ContainsSafeShip = config.ContainsSafeShip;
		ContainsFlagship = config.ContainsFlagship;
		NotifiesAtEndpoint = config.NotifiesAtEndpoint;
	}


	private void SubscribeToApis()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += CloseAll;

		o.ApiReqMap_Start.ResponseReceived += InSortie;
		o.ApiReqMap_Next.ResponseReceived += InSortie;

		o.ApiGetMember_MapInfo.ResponseReceived += BeforeSortie;

		o.ApiReqSortie_BattleResult.ResponseReceived += BattleFinished;
		o.ApiReqCombinedBattle_BattleResult.ResponseReceived += BattleFinished;

		o.ApiReqSortie_Battle.ResponseReceived += BattleStarted;
		o.ApiReqBattleMidnight_Battle.ResponseReceived += BattleStarted;
		o.ApiReqBattleMidnight_SpMidnight.ResponseReceived += BattleStarted;
		o.ApiReqSortie_AirBattle.ResponseReceived += BattleStarted;
		o.ApiReqSortie_LdAirBattle.ResponseReceived += BattleStarted;
		o.ApiReqSortie_NightToDay.ResponseReceived += BattleStarted;
		o.ApiReqSortie_LdShooting.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_Battle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_BattleWater.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_AirBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_MidnightBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_SpMidnight.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_LdAirBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EcBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EcMidnightBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EcNightToDay.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EachBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EachBattleWater.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_LdShooting.ResponseReceived += BattleStarted;
	}

	void CloseAll(string apiname, dynamic data)
	{
		DialogData.OnCloseAll();
	}




	private void BeforeSortie(string apiname, dynamic data)
	{
		if (NotifiesNow || NotifiesBefore)
		{

			string[] array = GetDamagedShips(
				KCDatabase.Instance.Fleet.Fleets.Values
					.Where(f => f.ExpeditionState == 0)
					.SelectMany(f => f.MembersWithoutEscaped.Skip(!ContainsFlagship ? 1 : 0)));

			if (array != null && array.Length > 0)
			{
				Notify(array);
			}
		}
	}


	private void InSortie(string apiname, dynamic data)
	{
		if (NotifiesAfter)
		{

			string[] array = GetDamagedShips(KCDatabase.Instance.Fleet.Fleets.Values
				.Where(f => f.IsInSortie)
				.SelectMany(f => f.MembersWithoutEscaped.Skip(!ContainsFlagship ? 1 : 0)));


			if (array != null && array.Length > 0)
			{
				Notify(array);
			}
		}
	}


	private void BattleStarted(string apiname, dynamic data)
	{
		if (NotifiesBefore)
		{
			CheckBattle();
		}
	}


	private void BattleFinished(string apiname, dynamic data)
	{
		if (NotifiesNow)
		{
			CheckBattle();
		}
	}


	private void CheckBattle()
	{

		BattleManager bm = KCDatabase.Instance.Battle;

		if (bm.Compass.IsEndPoint && !NotifiesAtEndpoint)
			return;


		var list = new List<string>();

		var battle = bm.SecondBattle ?? bm.FirstBattle;
		list.AddRange(GetDamagedShips(battle.Initial.FriendFleet, battle.ResultHPs.ToArray()));

		if (bm.IsCombinedBattle)
			list.AddRange(GetDamagedShips(battle.Initial.FriendFleetEscort, battle.ResultHPs.Skip(6).ToArray()));


		if (list.Count > 0)
			Notify(list.ToArray());

	}


	// 注: 退避中かどうかまではチェックしない
	private bool IsShipDamaged(IShipData ship, int hp)
	{
		return ship != null &&
			   hp > 0 &&
			   (double)hp / ship.HPMax <= 0.25 &&
			   ship.RepairingDockID == -1 &&
			   ship.Level >= LevelBorder &&
			   (ContainsNotLockedShip ? true : (ship.IsLocked || ship.SlotInstance.Count(q => q != null && q.IsLocked) > 0)) &&
			   (ContainsSafeShip ? true : !ship.AllSlotInstanceMaster.Any(e => e?.CategoryType == EquipmentTypes.DamageControl));
	}

	private string[] GetDamagedShips(IEnumerable<IShipData> ships)
	{
		return ships.Where(s => IsShipDamaged(s, s?.HPCurrent ?? 0)).Select(s => $"{s.NameWithLevel} ({s.HPCurrent}/{s.HPMax})").ToArray();
	}

	private string[] GetDamagedShips(FleetData fleet, int[] hps)
	{

		LinkedList<string> list = new LinkedList<string>();

		for (int i = 0; i < fleet.Members.Count; i++)
		{
			if (i == 0 && !ContainsFlagship) continue;

			IShipData s = fleet.MembersInstance[i];

			if (s != null && !fleet.EscapedShipList.Contains(s.MasterID) && IsShipDamaged(s, hps[i]))
			{
				list.AddLast($"{s.NameWithLevel} ({hps[i]}/{s.HPMax})");
			}
		}

		return list.ToArray();
	}

	private void Notify(string[] messages)
	{

		DialogData.Message = string.Format(NotifierRes.DamagedText,
			string.Join(", ", messages));

		base.Notify();
	}


	public override void ApplyToConfiguration(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
	{
		base.ApplyToConfiguration(config);


		if (config is Utility.Configuration.ConfigurationData.ConfigNotifierDamage c)
		{
			c.NotifiesBefore = NotifiesBefore;
			c.NotifiesNow = NotifiesNow;
			c.NotifiesAfter = NotifiesAfter;
			c.LevelBorder = LevelBorder;
			c.ContainsNotLockedShip = ContainsNotLockedShip;
			c.ContainsSafeShip = ContainsSafeShip;
			c.ContainsFlagship = ContainsFlagship;
			c.NotifiesAtEndpoint = NotifiesAtEndpoint;
		}
	}

}
