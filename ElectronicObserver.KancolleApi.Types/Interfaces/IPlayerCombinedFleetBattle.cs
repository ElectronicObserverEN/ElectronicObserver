namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IPlayerCombinedFleetBattle : IBattleApiResponse
{
	List<List<int>> ApiFParamCombined { get; set; }

	List<int> ApiFMaxhpsCombined { get; set; }

	List<int> ApiFNowhpsCombined { get; set; }

	List<int>? ApiEscapeIdxCombined { get; set; }

	/// <summary>
	/// 随伴艦隊戦闘糧食補給　発動時のみ存在　艦船IDの数値配列
	/// </summary>
	List<int>? ApiCombatRationCombined { get; }
}
