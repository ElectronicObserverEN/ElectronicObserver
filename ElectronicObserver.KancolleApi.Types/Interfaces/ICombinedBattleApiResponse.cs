namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface ICombinedBattleApiResponse : IBattleApiResponse
{
	List<List<int>> ApiEParamCombined { get; set; }

	List<List<int>> ApiESlotCombined { get; set; }

	List<int> ApiEMaxhpsCombined { get; set; }

	List<int> ApiENowhpsCombined { get; set; }

	List<int>? ApiEscapeIdxCombined { get; set; }

	List<List<int>> ApiFParamCombined { get; set; }

	List<int> ApiFMaxhpsCombined { get; set; }

	List<int> ApiFNowhpsCombined { get; set; }

	List<int> ApiShipKeCombined { get; set; }

	List<int> ApiShipLvCombined { get; set; }
}
