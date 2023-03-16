using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IDayBattleApiResponse : IBattleApiResponse, IAirBaseBattle
{
	List<ApiFlavorInfo>? ApiFlavorInfo { get; set; }

	/// <summary>
	/// 砲撃戦1巡目　api_hourai_flag[0] = 0 の時 null　メンバの配列はすべて [攻撃フェイズの回数]
	/// </summary>
	ApiHougeki1? ApiHougeki1 { get; set; }

	/// <summary>
	/// 砲撃戦2巡目　api_hourai_flag[1] = 0 の時 null　フォーマットは1巡目と同じ
	/// </summary>
	ApiHougeki1? ApiHougeki2 { get; set; }

	/// <summary>
	/// 砲撃戦3巡目　api_hourai_flag[2] = 0 の時 null　フォーマットは1巡目と同じ
	/// </summary>
	ApiHougeki1? ApiHougeki3 { get; set; }

	/// <summary>
	/// 砲雷撃戦フラグ　[4]
	/// </summary>
	List<int> ApiHouraiFlag { get; set; }

	/// <summary>
	/// 噴式強襲航空攻撃
	/// </summary>
	ApiInjectionKouku? ApiInjectionKouku { get; set; }

	/// <summary>
	/// 航空戦情報
	/// </summary>
	ApiKouku ApiKouku { get; set; }

	/// <summary>
	/// 夜戦突入可否　0=不可(昼戦で敵全滅など), 1=可能
	/// </summary>
	int ApiMidnightFlag { get; set; }

	/// <summary>
	/// 開幕雷撃戦 api_opening_flag = 0 の時 null　中身はおそらく api_raigeki と同じ *スペルミス注意*
	/// </summary>
	ApiOpeningAtack? ApiOpeningAtack { get; set; }

	/// <summary>
	/// 開幕雷撃フラグ 0=発動せず, 1=発動
	/// </summary>
	int ApiOpeningFlag { get; set; }

	/// <summary>
	/// 開幕対潜攻撃　上の=0のとき null　メンバの配列はすべて [攻撃フェイズの回数]　フォーマットは砲撃戦と同じ
	/// </summary>
	ApiOpeningTaisen ApiOpeningTaisen { get; set; }

	/// <summary>
	/// 開幕対潜攻撃フラグ 0=なし 1=あり
	/// </summary>
	int ApiOpeningTaisenFlag { get; set; }

	/// <summary>
	/// 雷撃戦　api_hourai_flag[3] = 0 の時 null
	/// </summary>
	ApiRaigekiClass? ApiRaigeki { get; set; }

	/// <summary>
	/// 索敵成否　[0]=味方, [1]=敵　1=成功, 2=成功(未帰還機あり), 3=未帰還, 4=失敗, 5=発見, 6=発見できず
	/// </summary>
	List<DetectionType> ApiSearch { get; set; }

	/// <summary>
	/// 航空戦フラグ　[n]=0 のとき api_stage<n+1>=null になる(航空戦力なし, 艦戦のみなど)
	/// </summary>
	List<int> ApiStageFlag { get; set; }

	/// <summary>
	/// 支援艦隊フラグ　0=なし, 1=空撃, 2=砲撃, 3=雷撃, 4=対潜
	/// </summary>
	int ApiSupportFlag { get; set; }

	/// <summary>
	/// 支援艦隊情報　api_support_flag = 0 なら null
	/// </summary>
	ApiSupportInfo? ApiSupportInfo { get; set; }

	// todo: api_escape_idx
	// todo: api_friendly_info
	// todo: api_friendly_battle
	// todo: api_friendly_kouku
}
