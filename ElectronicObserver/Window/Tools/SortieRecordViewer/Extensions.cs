using System;
using System.Text.Json;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiDmmPayment.Paycheck;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Basic;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Deck;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Mapinfo;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Material;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Mission;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Ndock;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Payitem;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.PictureBook;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Practice;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.PresetDeck;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.PresetSlot;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Record;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.SortieConditions;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Useitem;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.ChangeDeploymentBase;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.SetAction;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.SetPlane;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Supply;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.BattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.GobackPort;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqFurniture.Buy;
using ElectronicObserver.KancolleApi.Types.ApiReqHensei.Change;
using ElectronicObserver.KancolleApi.Types.ApiReqHensei.Combined;
using ElectronicObserver.KancolleApi.Types.ApiReqHensei.Lock;
using ElectronicObserver.KancolleApi.Types.ApiReqHensei.PresetRegister;
using ElectronicObserver.KancolleApi.Types.ApiReqHensei.PresetSelect;
using ElectronicObserver.KancolleApi.Types.ApiReqHokyu.Charge;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.CanPresetSlotSelect;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Lock;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Marriage;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.OpenExslot;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Powerup;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotRegister;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotSelect;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Remodeling;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotDeprive;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotExchangeIndex;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Slotset;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotsetEx;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.UnsetslotAll;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createitem;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createship;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.CreateshipSpeedchange;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyitem2;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyship;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Getship;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlot;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlist;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlistDetail;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.SelectEventmapRank;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.StartAirBase;
using ElectronicObserver.KancolleApi.Types.ApiReqMember.GetEventSelectedReward;
using ElectronicObserver.KancolleApi.Types.ApiReqMember.GetIncentive;
using ElectronicObserver.KancolleApi.Types.ApiReqMember.GetPracticeEnemyinfo;
using ElectronicObserver.KancolleApi.Types.ApiReqMember.Itemuse;
using ElectronicObserver.KancolleApi.Types.ApiReqMember.Payitemuse;
using ElectronicObserver.KancolleApi.Types.ApiReqMember.SetFlagshipPosition;
using ElectronicObserver.KancolleApi.Types.ApiReqMember.SetFriendlyRequest;
using ElectronicObserver.KancolleApi.Types.ApiReqMember.SetOssCondition;
using ElectronicObserver.KancolleApi.Types.ApiReqMember.Updatecomment;
using ElectronicObserver.KancolleApi.Types.ApiReqMember.Updatedeckname;
using ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;
using ElectronicObserver.KancolleApi.Types.ApiReqMission.ReturnInstruction;
using ElectronicObserver.KancolleApi.Types.ApiReqMission.Start;
using ElectronicObserver.KancolleApi.Types.ApiReqNyukyo.Speedchange;
using ElectronicObserver.KancolleApi.Types.ApiReqNyukyo.Start;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.ChangeMatchingKind;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.MidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqQuest.Clearitemget;
using ElectronicObserver.KancolleApi.Types.ApiReqQuest.Start;
using ElectronicObserver.KancolleApi.Types.ApiReqQuest.Stop;
using ElectronicObserver.KancolleApi.Types.ApiReqRanking.Mxltvkpyuklh;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Airbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.GoBackPort;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public static class Extensions
{
	public static IMapProgressApi? GetMapProgressApiData(this ApiFile apiFile) => apiFile.Name switch
	{
		"api_req_map/start" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapStartResponse>>(apiFile.Content)?.ApiData,
		"api_req_map/next" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapNextResponse>>(apiFile.Content)?.ApiData,

		_ => throw new NotImplementedException(),
	};

	public static ISortieBattleResultApi? GetSortieBattleResultApi(this ApiFile apiFile) => apiFile.Name switch
	{
		"api_req_sortie/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieBattleresultResponse>>(apiFile.Content)?.ApiData,
		"api_req_combined_battle/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleresultResponse>>(apiFile.Content)?.ApiData,

		_ => throw new NotImplementedException(),
	};

	public static object? GetResponseApiData(this Database.KancolleApi.ApiFile file) => file.Name switch
	{
		"api_dmm_payment/paycheck" => JsonSerializer.Deserialize<ApiResponse<ApiDmmPaymentPaycheckResponse>>(file.Content)?.ApiData,
		"api_get_member/basic" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberBasicResponse>>(file.Content)?.ApiData,
		"api_get_member/deck" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberDeckResponse>>(file.Content)?.ApiData,
		"api_get_member/kdock" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberKdockResponse>>(file.Content)?.ApiData,
		"api_get_member/mapinfo" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberMapinfoResponse>>(file.Content)?.ApiData,
		"api_get_member/material" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberMaterialResponse>>(file.Content)?.ApiData,
		"api_get_member/mission" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberMissionResponse>>(file.Content)?.ApiData,
		"api_get_member/ndock" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberNdockResponse>>(file.Content)?.ApiData,
		"api_get_member/payitem" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberPayitemResponse>>(file.Content)?.ApiData,
		"api_get_member/picture_book" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPictureBookResponse>>(file.Content)?.ApiData,
		"api_get_member/practice" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPracticeResponse>>(file.Content)?.ApiData,
		"api_get_member/preset_deck" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPresetDeckResponse>>(file.Content)?.ApiData,
		"api_get_member/preset_slot" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPresetSlotResponse>>(file.Content)?.ApiData,
		"api_get_member/record" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberRecordResponse>>(file.Content)?.ApiData,
		"api_get_member/ship_deck" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberShipDeckResponse>>(file.Content)?.ApiData,
		"api_get_member/sortie_conditions" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberSortieConditionsResponse>>(file.Content)?.ApiData,
		"api_get_member/useitem" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberUseitemResponse>>(file.Content)?.ApiData,
		"api_port/port" => JsonSerializer.Deserialize<ApiResponse<ApiPortPortResponse>>(file.Content)?.ApiData,
		"api_req_air_corps/change_deployment_base" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsChangeDeploymentBaseResponse>>(file.Content)?.ApiData,
		"api_req_air_corps/set_action" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsSetActionResponse>>(file.Content)?.ApiData,
		"api_req_air_corps/set_plane" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsSetPlaneResponse>>(file.Content)?.ApiData,
		"api_req_air_corps/supply" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsSupplyResponse>>(file.Content)?.ApiData,
		"api_req_battle_midnight/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqBattleMidnightBattleResponse>>(file.Content)?.ApiData,
		"api_req_battle_midnight/sp_midnight" => JsonSerializer.Deserialize<ApiResponse<ApiReqBattleMidnightSpMidnightResponse>>(file.Content)?.ApiData,
		"api_req_combined_battle/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleResponse>>(file.Content)?.ApiData,
		"api_req_combined_battle/battle_water" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleWaterResponse>>(file.Content)?.ApiData,
		"api_req_combined_battle/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleresultResponse>>(file.Content)?.ApiData,
		"api_req_combined_battle/each_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEachBattleResponse>>(file.Content)?.ApiData,
		"api_req_combined_battle/each_battle_water" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEachBattleWaterResponse>>(file.Content)?.ApiData,
		"api_req_combined_battle/ec_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEcBattleResponse>>(file.Content)?.ApiData,
		"api_req_combined_battle/ec_midnight_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEcMidnightBattleResponse>>(file.Content)?.ApiData,
		"api_req_combined_battle/goback_port" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleGobackPortResponse>>(file.Content)?.ApiData,
		"api_req_combined_battle/ld_airbattle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleLdAirbattleResponse>>(file.Content)?.ApiData,
		"api_req_combined_battle/midnight_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEcMidnightBattleResponse>>(file.Content)?.ApiData,
		"api_req_furniture/buy" => JsonSerializer.Deserialize<ApiResponse<ApiReqFurnitureBuyResponse>>(file.Content)?.ApiData,
		"api_req_hensei/change" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiChangeResponse>>(file.Content)?.ApiData,
		"api_req_hensei/combined" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiCombinedResponse>>(file.Content)?.ApiData,
		"api_req_hensei/lock" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiLockResponse>>(file.Content)?.ApiData,
		"api_req_hensei/preset_register" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiPresetRegisterResponse>>(file.Content)?.ApiData,
		"api_req_hensei/preset_select" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiPresetSelectResponse>>(file.Content)?.ApiData,
		"api_req_hokyu/charge" => JsonSerializer.Deserialize<ApiResponse<ApiReqHokyuChargeResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/can_preset_slot_select" => JsonSerializer.Deserialize<ApiResponseNull<APIReqKaisouCanPresetSlotSelectResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/lock" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouLockResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/marriage" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouMarriageResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/open_exslot" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouOpenExslotResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/powerup" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouPowerupResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/preset_slot_register" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqKaisouPresetSlotRegisterResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/preset_slot_select" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqKaisouPresetSlotSelectResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/remodeling" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouRemodelingResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/slot_deprive" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotDepriveResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/slot_exchange_index" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotExchangeIndexResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/slotset" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotsetResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/slotset_ex" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotsetExResponse>>(file.Content)?.ApiData,
		"api_req_kaisou/unsetslot_all" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouUnsetslotAllResponse>>(file.Content)?.ApiData,
		"api_req_kousyou/createitem" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouCreateitemResponse>>(file.Content)?.ApiData,
		"api_req_kousyou/createship" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouCreateshipResponse>>(file.Content)?.ApiData,
		"api_req_kousyou/createship_speedchange" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouCreateshipSpeedchangeResponse>>(file.Content)?.ApiData,
		"api_req_kousyou/destroyitem2" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouDestroyitem2Response>>(file.Content)?.ApiData,
		"api_req_kousyou/destroyship" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouDestroyshipResponse>>(file.Content)?.ApiData,
		"api_req_kousyou/getship" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouGetshipResponse>>(file.Content)?.ApiData,
		"api_req_kousyou/remodel_slot" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouRemodelSlotResponse>>(file.Content)?.ApiData,
		"api_req_kousyou/remodel_slotlist" => JsonSerializer.Deserialize<ApiResponseList<APIReqKousyouRemodelSlotlistResponse>>(file.Content)?.ApiData,
		"api_req_kousyou/remodel_slotlist_detail" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouRemodelSlotlistDetailResponse>>(file.Content)?.ApiData,
		"api_req_map/next" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapNextResponse>>(file.Content)?.ApiData,
		"api_req_map/select_eventmap_rank" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapSelectEventmapRankResponse>>(file.Content)?.ApiData,
		"api_req_map/start" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapStartResponse>>(file.Content)?.ApiData,
		"api_req_map/start_air_base" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapStartAirBaseResponse>>(file.Content)?.ApiData,
		"api_req_member/get_event_selected_reward" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberGetEventSelectedRewardResponse>>(file.Content)?.ApiData,
		"api_req_member/get_incentive" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberGetIncentiveResponse>>(file.Content)?.ApiData,
		"api_req_member/get_practice_enemyinfo" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberGetPracticeEnemyinfoResponse>>(file.Content)?.ApiData,
		"api_req_member/itemuse" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberItemuseResponse>>(file.Content)?.ApiData,
		"api_req_member/payitemuse" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberPayitemuseResponse>>(file.Content)?.ApiData,
		"api_req_member/set_flagship_position" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberSetFlagshipPositionResponse>>(file.Content)?.ApiData,
		"api_req_member/set_friendly_request" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqMemberSetFriendlyRequestResponse>>(file.Content)?.ApiData,
		"api_req_member/set_oss_condition" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberSetOssConditionResponse>>(file.Content)?.ApiData,
		"api_req_member/updatecomment" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqMemberUpdatecommentResponse>>(file.Content)?.ApiData,
		"api_req_member/updatedeckname" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqMemberUpdatedecknameResponse>>(file.Content)?.ApiData,
		"api_req_mission/result" => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionResultResponse>>(file.Content)?.ApiData,
		"api_req_mission/return_instruction" => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionReturnInstructionResponse>>(file.Content)?.ApiData,
		"api_req_mission/start" => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionStartResponse>>(file.Content)?.ApiData,
		"api_req_nyukyo/speedchange" => JsonSerializer.Deserialize<ApiResponse<ApiReqNyukyoSpeedchangeResponse>>(file.Content)?.ApiData,
		"api_req_nyukyo/start" => JsonSerializer.Deserialize<ApiResponseNull<APIReqNyukyoStartResponse>>(file.Content)?.ApiData,
		"api_req_practice/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeBattleResponse>>(file.Content)?.ApiData,
		"api_req_practice/battle_result" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeBattleResultResponse>>(file.Content)?.ApiData,
		"api_req_practice/change_matching_kind" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeChangeMatchingKindResponse>>(file.Content)?.ApiData,
		"api_req_practice/midnight_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeMidnightBattleResponse>>(file.Content)?.ApiData,
		"api_req_quest/clearitemget" => JsonSerializer.Deserialize<ApiResponse<ApiReqQuestClearitemgetResponse>>(file.Content)?.ApiData,
		"api_req_quest/start" => JsonSerializer.Deserialize<ApiResponse<ApiReqQuestStartResponse>>(file.Content)?.ApiData,
		"api_req_quest/stop" => JsonSerializer.Deserialize<ApiResponse<ApiReqQuestStopResponse>>(file.Content)?.ApiData,
		"api_req_ranking/mxltvkpyuklh" => JsonSerializer.Deserialize<ApiResponse<ApiReqRankingMxltvkpyuklhResponse>>(file.Content)?.ApiData,
		"api_req_sortie/airbattle" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieAirbattleResponse>>(file.Content)?.ApiData,
		"api_req_sortie/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieBattleResponse>>(file.Content)?.ApiData,
		"api_req_sortie/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieBattleresultResponse>>(file.Content)?.ApiData,
		"api_req_sortie/goback_port" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqSortieGobackPortResponse>>(file.Content)?.ApiData,
		"api_req_sortie/ld_airbattle" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieLdAirbattleResponse>>(file.Content)?.ApiData,

		_ => throw new NotImplementedException(),
	};
}
