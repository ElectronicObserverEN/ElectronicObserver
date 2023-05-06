namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IPlayerCombinedFleetBattle : IBattleApiResponse
{
	List<List<int>> ApiFParamCombined { get; set; }

	List<int> ApiFMaxhpsCombined { get; set; }

	List<int> ApiFNowhpsCombined { get; set; }

	List<int>? ApiEscapeIdxCombined { get; set; }
}