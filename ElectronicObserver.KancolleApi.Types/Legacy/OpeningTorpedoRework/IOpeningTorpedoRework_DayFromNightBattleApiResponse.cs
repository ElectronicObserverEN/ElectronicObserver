using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Legacy.OpeningTorpedoRework;

public interface IOpeningTorpedoRework_DayFromNightBattleApiResponse : INightBattleApiResponse, IOpeningTorpedoRework_DayBattleApiResponse
{
	/// <summary>
	/// 夜間砲雷撃戦　1巡目　味方(随伴)?->敵全体 vs 敵随伴->味方?
	/// </summary>
	ApiHougeki? ApiNHougeki1 { get; set; }

	/// <summary>
	/// 夜間砲雷撃戦　2巡目　味方(本体)?->敵全体 vs 敵本体->味方?
	/// </summary>
	ApiHougeki? ApiNHougeki2 { get; set; }

	/// <summary>
	/// 昼戦可否　0=不可, 1=可　以下のメンバ(api_escape_idx を除く)は 1 の時のみ存在
	/// </summary>
	int ApiDayFlag { get; set; }
}
