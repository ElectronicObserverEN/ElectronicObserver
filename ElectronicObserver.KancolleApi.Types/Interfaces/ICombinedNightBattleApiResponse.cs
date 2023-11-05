namespace ElectronicObserver.KancolleApi.Types.Interfaces;

/// <summary>
/// Player fleet can be normal or combined here.
/// </summary>
public interface ICombinedNightBattleApiResponse : INightGearApiResponse, IEnemyCombinedFleetBattle
{
	List<List<int>>? ApiFParamCombined { get; set; }

	List<int>? ApiFMaxhpsCombined { get; set; }

	List<int>? ApiFNowhpsCombined { get; set; }

	List<int>? ApiEscapeIdxCombined { get; set; }

	/// <summary>
	/// 戦闘参加艦隊フラグ　[2]　[0]=味方側, [1]=敵側　1=主力艦隊, 2=随伴艦隊
	/// </summary>
	List<int> ApiActiveDeck { get; set; }
}
