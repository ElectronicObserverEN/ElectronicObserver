namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface ICombinedNightBattleApiResponse : INightGearApiResponse
{
	/// <summary>
	/// 戦闘参加艦隊フラグ　[2]　[0]=味方側, [1]=敵側　1=主力艦隊, 2=随伴艦隊
	/// </summary>
	List<int> ApiActiveDeck { get; set; }
}
