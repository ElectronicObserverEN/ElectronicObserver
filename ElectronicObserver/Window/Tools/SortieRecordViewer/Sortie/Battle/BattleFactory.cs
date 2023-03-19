using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public class BattleFactory
{
	private IKCDatabase KcDatabase { get; }

	public BattleFactory(IKCDatabase kcDatabase)
	{
		KcDatabase = kcDatabase;
	}

	public BattleNormalDay CreateBattle(IFleetData fleet, ApiReqSortieBattleResponse battle)
		=> new(KcDatabase, new(fleet), battle);

	public BattleNightOnly CreateBattle(IFleetData fleet, ApiReqBattleMidnightSpMidnightResponse battle)
		=> new(KcDatabase, new(fleet), battle);
}
