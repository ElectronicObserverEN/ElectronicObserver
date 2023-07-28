namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IEnemyCombinedFleetBattle : IBattleApiResponse
{
	List<List<int>> ApiEParamCombined { get; set; }

	List<List<int>> ApiESlotCombined { get; set; }

	List<int> ApiEMaxhpsCombined { get; set; }

	List<int> ApiENowhpsCombined { get; set; }

	List<int> ApiShipKeCombined { get; set; }

	List<int> ApiShipLvCombined { get; set; }
}