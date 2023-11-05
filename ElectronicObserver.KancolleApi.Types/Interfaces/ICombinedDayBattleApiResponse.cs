using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface ICombinedDayBattleApiResponse : IDayBattleApiResponse
{
	/// <summary>
	/// 砲撃戦3巡目　api_hourai_flag[2] = 0 の時 null　フォーマットは1巡目と同じ
	/// </summary>
	ApiHougeki1? ApiHougeki3 { get; set; }
}
