using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Airbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdAirbattle;
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

	public BattleEnemyCombinedNight CreateBattle(ApiReqCombinedBattleEcMidnightBattleResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);

	public BattleCombinedEachWater CreateBattle(ApiReqCombinedBattleEachBattleWaterResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);

	public BattleAirBattle CreateBattle(ApiReqSortieAirbattleResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);

	public BattleAirRaid CreateBattle(ApiReqSortieLdAirbattleResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);

	public BattleCombinedAirRaid CreateBattle(ApiReqCombinedBattleLdAirbattleResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);

	public BattleCombinedNightOnly CreateBattle(ApiReqCombinedBattleSpMidnightResponse battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);

	public BattleBaseAirRaid CreateBattle(ApiDestructionBattle battle, BattleFleets fleets)
		=> new(KcDatabase, fleets, battle);
}
