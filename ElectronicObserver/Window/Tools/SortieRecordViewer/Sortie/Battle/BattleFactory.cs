using System.Collections.Generic;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public class BattleFactory
{
	private IKCDatabase KcDatabase { get; }

	public BattleFactory(IKCDatabase kcDatabase)
	{
		KcDatabase = kcDatabase;
	}

	public BattleNormalDay CreateBattle(ApiReqSortieBattleResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);

	public BattleNormalNight CreateBattle(ApiReqBattleMidnightBattleResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);

	public BattleNightOnly CreateBattle(ApiReqBattleMidnightSpMidnightResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);

	public BattleCombinedEachDay CreateBattle(ApiReqCombinedBattleEachBattleResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);

	public BattleEnemyCombinedDay CreateBattle(ApiReqCombinedBattleEcBattleResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);
}
