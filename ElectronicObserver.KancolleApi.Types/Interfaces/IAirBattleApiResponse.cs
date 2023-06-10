using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IAirBattleApiResponse : IFirstBattleApiResponse, IDaySearch, IAirBaseBattle, IDayFriendFleetApiResponse
{
	/// <summary>
	/// 航空戦フラグ　[n]=0 のとき api_stage<n+1>=null になる(航空戦力なし, 艦戦のみなど)
	/// </summary>
	List<int> ApiStageFlag { get; set; }

	/// <summary>
	/// 噴式強襲航空攻撃
	/// </summary>
	ApiInjectionKouku? ApiInjectionKouku { get; set; }

	/// <summary>
	/// 航空戦情報
	/// </summary>
	ApiKouku? ApiKouku { get; set; }
}
