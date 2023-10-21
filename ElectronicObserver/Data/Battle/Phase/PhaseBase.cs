using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data.Battle.Detail;

namespace ElectronicObserver.Data.Battle.Phase;

/// <summary>
/// 戦闘フェーズの基底クラスです。
/// </summary>
public abstract class PhaseBase
{
	protected BattleData Battle;
	public List<BattleDetail> BattleDetails { get; protected set; }
	public readonly string Title;


	private string TranslatedPhaseTitle(string title) => title switch
	{
		"噴式基地航空隊攻撃" => BattleRes.BattlePhaseLandBasedJet,
		"噴式航空戦" => BattleRes.BattlePhaseJet,
		"基地航空隊攻撃" => BattleRes.BattlePhaseLandBasedAir,
		"防空戦" => BattleRes.BattlePhaseAirBaseRaid,
		"航空戦" => BattleRes.BattlePhaseAirBattle,
		"空襲戦" => BattleRes.BattlePhaseAirRaid,
		"第一次航空戦" => BattleRes.BattlePhaseAirAttackFirst,
		"第二次航空戦" => BattleRes.BattlePhaseAirAttackSecond,
		"支援攻撃" => BattleRes.BattlePhaseSupportExpedition,
		"先制対潜" => BattleRes.BattlePhaseOpeningAsw,
		"先制雷撃" => BattleRes.BattlePhaseOpeningTorpedo,
		"第一次砲撃戦" => BattleRes.BattlePhaseShellingFirst,
		"第二次砲撃戦" => BattleRes.BattlePhaseShellingSecond,
		"第三次砲撃戦" => BattleRes.BattlePhaseShellingThird,
		"雷撃戦" => BattleRes.BattlePhaseClosingTorpedo,
		"夜戦" => BattleRes.BattlePhaseNightBattle,
		"第一次夜戦" => BattleRes.BattlePhaseNightBattleFirst,
		"第二次夜戦" => BattleRes.BattlePhaseNightBattleSecond,
		"夜間支援攻撃" => BattleRes.BattlePhaseNightSupportExpedition,

		_ => title,
	};

	protected PhaseBase(BattleData battle, string title)
	{
		Battle = battle;
		BattleDetails = new List<BattleDetail>();
		Title = TranslatedPhaseTitle(title);
	}


	protected dynamic RawData => Battle.RawData;


	protected static bool IsIndexFriend(int index) => 0 <= index && index < 12;
	protected static bool IsIndexEnemy(int index) => 12 <= index && index < 24;


	/// <summary>
	/// 被ダメージ処理を行います。
	/// </summary>
	/// <param name="hps">各艦のHPリスト。</param>
	/// <param name="index">ダメージを受ける艦のインデックス。</param>
	/// <param name="damage">ダメージ。</param>
	protected void AddDamage(int[] hps, int index, int damage)
	{

		hps[index] -= Math.Max(damage, 0);

		// 自軍艦の撃沈が発生した場合(ダメコン処理)
		if (hps[index] <= 0 && IsIndexFriend(index) && !Battle.IsPractice)
		{
			var ship = Battle.Initial.GetFriendShip(index);
			if (ship == null)
				return;

			int id = ship.DamageControlID;

			if (id == 42)
				hps[index] = (int)(ship.HPMax * 0.2);

			else if (id == 43)
				hps[index] = ship.HPMax;

		}
	}


	protected virtual IEnumerable<BattleDetail> SearchBattleDetails(int index)
	{
		return BattleDetails.Where(d => d.AttackerIndex == index || d.DefenderIndex == index);
	}
	public virtual string GetBattleDetail(int index)
	{
		IEnumerable<BattleDetail> list;
		if (index == -1)
			list = BattleDetails;
		else
			list = SearchBattleDetails(index);

		if (list.Any())
		{
			return string.Join("\r\n", list) + "\r\n";
		}
		else return null;
	}
	public virtual string GetBattleDetail() { return GetBattleDetail(-1); }


	public override string ToString() => string.Join(" / \r\n", BattleDetails);



	/// <summary>
	/// データが有効かどうかを示します。
	/// </summary>
	public abstract bool IsAvailable { get; }

	/// <summary>
	/// 戦闘をエミュレートします。
	/// </summary>
	/// <param name="hps">各艦のHPリスト。</param>
	/// <param name="damages">各艦の与ダメージリスト。</param>
	public abstract void EmulateBattle(int[] hps, int[] damages);


}
