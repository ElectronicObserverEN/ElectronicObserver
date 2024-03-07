using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Legacy.OpeningTorpedoRework;

// ReSharper disable once InconsistentNaming
[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Legacy code prefix")]
public interface IOpeningTorpedoRework_CombinedDayBattleApiResponse : IOpeningTorpedoRework_DayBattleApiResponse
{
	/// <summary>
	/// 砲撃戦3巡目　api_hourai_flag[2] = 0 の時 null　フォーマットは1巡目と同じ
	/// </summary>
	ApiHougeki1? ApiHougeki3 { get; set; }
}
