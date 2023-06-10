using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IDayBattleApiResponse : IAirBattleApiResponse, ISupportApiResponse
{
	/// <summary>
	/// 開幕対潜攻撃フラグ 0=なし 1=あり
	/// </summary>
	int ApiOpeningTaisenFlag { get; set; }

	/// <summary>
	/// 開幕対潜攻撃　上の=0のとき null　メンバの配列はすべて [攻撃フェイズの回数]　フォーマットは砲撃戦と同じ
	/// </summary>
	ApiHougeki1? ApiOpeningTaisen { get; set; }

	/// <summary>
	/// 開幕雷撃フラグ 0=発動せず, 1=発動
	/// </summary>
	int ApiOpeningFlag { get; set; }

	/// <summary>
	/// 開幕雷撃戦 api_opening_flag = 0 の時 null　中身はおそらく api_raigeki と同じ *スペルミス注意*
	/// </summary>
	ApiRaigekiClass? ApiOpeningAtack { get; set; }

	/// <summary>
	/// 砲雷撃戦フラグ　[4]
	/// </summary>
	List<int> ApiHouraiFlag { get; set; }

	/// <summary>
	/// 砲撃戦1巡目　<see cref="ApiHouraiFlag"/>[0] = 0 の時 null　メンバの配列はすべて [攻撃フェイズの回数]
	/// </summary>
	ApiHougeki1? ApiHougeki1 { get; set; }

	/// <summary>
	/// 砲撃戦2巡目　<see cref="ApiHouraiFlag"/>[1] = 0 の時 null　フォーマットは1巡目と同じ
	/// </summary>
	ApiHougeki1? ApiHougeki2 { get; set; }

	/// <summary>
	/// 雷撃戦　<see cref="ApiHouraiFlag"/>[3] = 0 の時 null
	/// </summary>
	ApiRaigekiClass? ApiRaigeki { get; set; }
}
