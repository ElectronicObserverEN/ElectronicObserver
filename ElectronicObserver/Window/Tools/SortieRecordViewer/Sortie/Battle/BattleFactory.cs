using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
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

	public BattleNormalDay CreateBattle(ApiReqSortieBattleResponse battle, IFleetData fleet)
		=> new(KcDatabase, new(fleet), battle);

	public BattleNormalNight CreateBattle(ApiReqBattleMidnightBattleResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);

	public BattleNightOnly CreateBattle(ApiReqBattleMidnightSpMidnightResponse battle, IFleetData fleet)
		=> new(KcDatabase, new(fleet), battle);

	public BattleCombinedEachDay CreateBattle(ApiReqCombinedBattleEachBattleResponse battle,
		IFleetData fleet, IFleetData escortFleet, List<IBaseAirCorpsData>? airBases)
		=> new(KcDatabase, new(fleet, escortFleet, airBases), battle);
}
