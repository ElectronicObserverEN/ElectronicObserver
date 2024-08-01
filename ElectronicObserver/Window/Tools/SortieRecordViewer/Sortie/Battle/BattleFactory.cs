using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Airbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.BattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcNightToDay;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdShooting;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.MidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.MidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Airbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdShooting;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.NightToDay;
using ElectronicObserver.KancolleApi.Types.Legacy.OpeningTorpedoRework;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public class BattleFactory(PhaseFactory phaseFactory)
{
	private PhaseFactory PhaseFactory { get; } = phaseFactory;

	public BattleNormalDay CreateBattle(ApiReqSortieBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleNormalNight CreateBattle(ApiReqBattleMidnightBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleNightOnly CreateBattle(ApiReqBattleMidnightSpMidnightResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedEachDay CreateBattle(ApiReqCombinedBattleEachBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleEnemyCombinedDay CreateBattle(ApiReqCombinedBattleEcBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleEnemyCombinedNight CreateBattle(ApiReqCombinedBattleEcMidnightBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedEachWater CreateBattle(ApiReqCombinedBattleEachBattleWaterResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleAirBattle CreateBattle(ApiReqSortieAirbattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleAirRaid CreateBattle(ApiReqSortieLdAirbattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedAirRaid CreateBattle(ApiReqCombinedBattleLdAirbattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedNightOnly CreateBattle(ApiReqCombinedBattleSpMidnightResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleBaseAirRaid CreateBattle(ApiDestructionBattle battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedNormalDay CreateBattle(ApiReqCombinedBattleBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedWater CreateBattle(ApiReqCombinedBattleBattleWaterResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedNormalNight CreateBattle(ApiReqCombinedBattleMidnightBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleNormalRadar CreateBattle(ApiReqSortieLdShootingResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedRadar CreateBattle(ApiReqCombinedBattleLdShootingResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleNormalDay CreateBattle(OpeningTorpedoRework_ApiReqSortieBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedNormalDay CreateBattle(OpeningTorpedoRework_ApiReqCombinedBattleBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedWater CreateBattle(OpeningTorpedoRework_ApiReqCombinedBattleBattleWaterResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleEnemyCombinedDay CreateBattle(OpeningTorpedoRework_ApiReqCombinedBattleEcBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedEachDay CreateBattle(OpeningTorpedoRework_ApiReqCombinedBattleEachBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedEachWater CreateBattle(OpeningTorpedoRework_ApiReqCombinedBattleEachBattleWaterResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleNormalDayFromNight CreateBattle(ApiReqSortieNightToDayResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleEnemyCombinedDayFromNight CreateBattle(ApiReqCombinedBattleEcNightToDayResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattleCombinedAirBattle CreateBattle(ApiReqCombinedBattleAirbattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattlePracticeDay CreateBattle(ApiReqPracticeBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);

	public BattlePracticeNight CreateBattle(ApiReqPracticeMidnightBattleResponse battle, BattleFleets fleets)
		=> new(PhaseFactory, fleets, battle);
}
