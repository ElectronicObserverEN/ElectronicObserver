using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ElectronicObserver.Database;
using ElectronicObserver.Database.Expedition;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
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
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdShooting;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.MidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.SpMidnight;
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
using ElectronicObserver.KancolleApi.Types.ApiReqMap.AnchorageRepair;
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
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdShooting;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Legacy.OpeningTorpedoRework;
using ElectronicObserver.KancolleApi.Types.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public static class Extensions
{
	// maintenance was 11:00 JST, this is 12:00 JST because it's still possible to get data after 11:00
	private static DateTime OpeningTorpedoRework { get; } = new(2024, 02, 29, 03, 00, 00, DateTimeKind.Utc);

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

	public static object? GetRequestApiData(this ApiFile file) => file.Name switch
	{
		"api_dmm_payment/paycheck" => JsonSerializer.Deserialize<ApiDmmPaymentPaycheckRequest>(file.Content),
		"api_get_member/basic" => JsonSerializer.Deserialize<ApiGetMemberBasicRequest>(file.Content),
		"api_get_member/deck" => JsonSerializer.Deserialize<ApiGetMemberDeckRequest>(file.Content),
		"api_get_member/kdock" => JsonSerializer.Deserialize<ApiGetMemberKdockRequest>(file.Content),
		"api_get_member/mapinfo" => JsonSerializer.Deserialize<ApiGetMemberMapinfoRequest>(file.Content),
		"api_get_member/material" => JsonSerializer.Deserialize<ApiGetMemberMaterialRequest>(file.Content),
		"api_get_member/mission" => JsonSerializer.Deserialize<ApiGetMemberMissionRequest>(file.Content),
		"api_get_member/ndock" => JsonSerializer.Deserialize<ApiGetMemberNdockRequest>(file.Content),
		"api_get_member/payitem" => JsonSerializer.Deserialize<ApiGetMemberPayitemRequest>(file.Content),
		"api_get_member/picture_book" => JsonSerializer.Deserialize<ApiGetMemberPictureBookRequest>(file.Content),
		"api_get_member/practice" => JsonSerializer.Deserialize<ApiGetMemberPracticeRequest>(file.Content),
		"api_get_member/preset_deck" => JsonSerializer.Deserialize<ApiGetMemberPresetDeckRequest>(file.Content),
		"api_get_member/preset_slot" => JsonSerializer.Deserialize<ApiGetMemberPresetSlotRequest>(file.Content),
		"api_get_member/record" => JsonSerializer.Deserialize<ApiGetMemberRecordRequest>(file.Content),
		"api_get_member/ship_deck" => JsonSerializer.Deserialize<ApiGetMemberShipDeckRequest>(file.Content),
		"api_get_member/sortie_conditions" => JsonSerializer.Deserialize<ApiGetMemberSortieConditionsRequest>(file.Content),
		"api_get_member/useitem" => JsonSerializer.Deserialize<ApiGetMemberUseitemRequest>(file.Content),
		"api_port/port" => JsonSerializer.Deserialize<ApiPortPortRequest>(file.Content),
		"api_req_air_corps/change_deployment_base" => JsonSerializer.Deserialize<ApiReqAirCorpsChangeDeploymentBaseRequest>(file.Content),
		"api_req_air_corps/set_action" => JsonSerializer.Deserialize<ApiReqAirCorpsSetActionRequest>(file.Content),
		"api_req_air_corps/set_plane" => JsonSerializer.Deserialize<ApiReqAirCorpsSetPlaneRequest>(file.Content),
		"api_req_air_corps/supply" => JsonSerializer.Deserialize<ApiReqAirCorpsSupplyRequest>(file.Content),
		"api_req_combined_battle/battleresult" => JsonSerializer.Deserialize<ApiReqCombinedBattleBattleresultRequest>(file.Content),
		"api_req_combined_battle/goback_port" => JsonSerializer.Deserialize<ApiReqCombinedBattleGobackPortRequest>(file.Content),
		"api_req_furniture/buy" => JsonSerializer.Deserialize<ApiReqFurnitureBuyRequest>(file.Content),
		"api_req_hensei/change" => JsonSerializer.Deserialize<ApiReqHenseiChangeRequest>(file.Content),
		"api_req_hensei/combined" => JsonSerializer.Deserialize<ApiReqHenseiCombinedRequest>(file.Content),
		"api_req_hensei/lock" => JsonSerializer.Deserialize<ApiReqHenseiLockRequest>(file.Content),
		"api_req_hensei/preset_register" => JsonSerializer.Deserialize<ApiReqHenseiPresetRegisterRequest>(file.Content),
		"api_req_hensei/preset_select" => JsonSerializer.Deserialize<ApiReqHenseiPresetSelectRequest>(file.Content),
		"api_req_hokyu/charge" => JsonSerializer.Deserialize<ApiReqHokyuChargeRequest>(file.Content),
		// "api_req_kaisou/can_preset_slot_select" => JsonSerializer.Deserialize<APIReqKaisouCanPresetSlotSelectRequest>(file.Content),
		"api_req_kaisou/lock" => JsonSerializer.Deserialize<ApiReqKaisouLockRequest>(file.Content),
		"api_req_kaisou/marriage" => JsonSerializer.Deserialize<ApiReqKaisouMarriageRequest>(file.Content),
		"api_req_kaisou/open_exslot" => JsonSerializer.Deserialize<ApiReqKaisouOpenExslotRequest>(file.Content),
		"api_req_kaisou/powerup" => JsonSerializer.Deserialize<ApiReqKaisouPowerupRequest>(file.Content),
		"api_req_kaisou/preset_slot_register" => JsonSerializer.Deserialize<ApiReqKaisouPresetSlotRegisterRequest>(file.Content),
		"api_req_kaisou/preset_slot_select" => JsonSerializer.Deserialize<ApiReqKaisouPresetSlotSelectRequest>(file.Content),
		"api_req_kaisou/remodeling" => JsonSerializer.Deserialize<ApiReqKaisouRemodelingRequest>(file.Content),
		"api_req_kaisou/slot_deprive" => JsonSerializer.Deserialize<ApiReqKaisouSlotDepriveRequest>(file.Content),
		"api_req_kaisou/slot_exchange_index" => JsonSerializer.Deserialize<ApiReqKaisouSlotExchangeIndexRequest>(file.Content),
		"api_req_kaisou/slotset" => JsonSerializer.Deserialize<ApiReqKaisouSlotsetRequest>(file.Content),
		"api_req_kaisou/slotset_ex" => JsonSerializer.Deserialize<ApiReqKaisouSlotsetExRequest>(file.Content),
		"api_req_kaisou/unsetslot_all" => JsonSerializer.Deserialize<ApiReqKaisouUnsetslotAllRequest>(file.Content),
		"api_req_kousyou/createitem" => JsonSerializer.Deserialize<ApiReqKousyouCreateitemRequest>(file.Content),
		"api_req_kousyou/createship" => JsonSerializer.Deserialize<ApiReqKousyouCreateshipRequest>(file.Content),
		"api_req_kousyou/createship_speedchange" => JsonSerializer.Deserialize<ApiReqKousyouCreateshipSpeedchangeRequest>(file.Content),
		"api_req_kousyou/destroyitem2" => JsonSerializer.Deserialize<ApiReqKousyouDestroyitem2Request>(file.Content),
		"api_req_kousyou/destroyship" => JsonSerializer.Deserialize<ApiReqKousyouDestroyshipRequest>(file.Content),
		"api_req_kousyou/getship" => JsonSerializer.Deserialize<ApiReqKousyouGetshipRequest>(file.Content),
		"api_req_kousyou/remodel_slot" => JsonSerializer.Deserialize<ApiReqKousyouRemodelSlotRequest>(file.Content),
		// "api_req_kousyou/remodel_slotlist" => JsonSerializer.Deserialize<APIReqKousyouRemodelSlotlistRequest>(file.Content),
		"api_req_kousyou/remodel_slotlist_detail" => JsonSerializer.Deserialize<ApiReqKousyouRemodelSlotlistDetailRequest>(file.Content),
		"api_req_map/next" => JsonSerializer.Deserialize<ApiReqMapNextRequest>(file.Content),
		"api_req_map/select_eventmap_rank" => JsonSerializer.Deserialize<ApiReqMapSelectEventmapRankRequest>(file.Content),
		"api_req_map/start" => JsonSerializer.Deserialize<ApiReqMapStartRequest>(file.Content),
		"api_req_map/start_air_base" => JsonSerializer.Deserialize<ApiReqMapStartAirBaseRequest>(file.Content),
		"api_req_member/get_event_selected_reward" => JsonSerializer.Deserialize<ApiReqMemberGetEventSelectedRewardRequest>(file.Content),
		"api_req_member/get_incentive" => JsonSerializer.Deserialize<ApiReqMemberGetIncentiveRequest>(file.Content),
		"api_req_member/get_practice_enemyinfo" => JsonSerializer.Deserialize<ApiReqMemberGetPracticeEnemyinfoRequest>(file.Content),
		"api_req_member/itemuse" => JsonSerializer.Deserialize<ApiReqMemberItemuseRequest>(file.Content),
		"api_req_member/payitemuse" => JsonSerializer.Deserialize<ApiReqMemberPayitemuseRequest>(file.Content),
		"api_req_member/set_flagship_position" => JsonSerializer.Deserialize<ApiReqMemberSetFlagshipPositionRequest>(file.Content),
		"api_req_member/set_friendly_request" => JsonSerializer.Deserialize<ApiReqMemberSetFriendlyRequestRequest>(file.Content),
		"api_req_member/set_oss_condition" => JsonSerializer.Deserialize<ApiReqMemberSetOssConditionRequest>(file.Content),
		"api_req_member/updatecomment" => JsonSerializer.Deserialize<ApiReqMemberUpdatecommentRequest>(file.Content),
		"api_req_member/updatedeckname" => JsonSerializer.Deserialize<ApiReqMemberUpdatedecknameRequest>(file.Content),
		"api_req_mission/result" => JsonSerializer.Deserialize<ApiReqMissionResultRequest>(file.Content),
		"api_req_mission/return_instruction" => JsonSerializer.Deserialize<ApiReqMissionReturnInstructionRequest>(file.Content),
		"api_req_mission/start" => JsonSerializer.Deserialize<ApiReqMissionStartRequest>(file.Content),
		"api_req_nyukyo/speedchange" => JsonSerializer.Deserialize<ApiReqNyukyoSpeedchangeRequest>(file.Content),
		// "api_req_nyukyo/start" => JsonSerializer.Deserialize<APIReqNyukyoStartRequest>(file.Content),
		"api_req_practice/battle" => JsonSerializer.Deserialize<ApiReqPracticeBattleRequest>(file.Content),
		"api_req_practice/battle_result" => JsonSerializer.Deserialize<ApiReqPracticeBattleResultRequest>(file.Content),
		"api_req_practice/change_matching_kind" => JsonSerializer.Deserialize<ApiReqPracticeChangeMatchingKindRequest>(file.Content),
		"api_req_practice/midnight_battle" => JsonSerializer.Deserialize<ApiReqPracticeMidnightBattleRequest>(file.Content),
		"api_req_quest/clearitemget" => JsonSerializer.Deserialize<ApiReqQuestClearitemgetRequest>(file.Content),
		"api_req_quest/start" => JsonSerializer.Deserialize<ApiReqQuestStartRequest>(file.Content),
		"api_req_quest/stop" => JsonSerializer.Deserialize<ApiReqQuestStopRequest>(file.Content),
		"api_req_ranking/mxltvkpyuklh" => JsonSerializer.Deserialize<ApiReqRankingMxltvkpyuklhRequest>(file.Content),
		"api_req_sortie/battleresult" => JsonSerializer.Deserialize<ApiReqSortieBattleresultRequest>(file.Content),
		"api_req_sortie/goback_port" => JsonSerializer.Deserialize<ApiReqSortieGobackPortRequest>(file.Content),

		"api_req_sortie/battle" => JsonSerializer.Deserialize<ApiReqSortieBattleRequest>(file.Content),
		"api_req_battle_midnight/sp_midnight" => JsonSerializer.Deserialize<ApiReqBattleMidnightSpMidnightRequest>(file.Content),
		"api_req_sortie/airbattle" => JsonSerializer.Deserialize<ApiReqSortieAirbattleRequest>(file.Content),
		"api_req_sortie/ld_airbattle" => JsonSerializer.Deserialize<ApiReqSortieLdAirbattleRequest>(file.Content),
		"api_req_sortie/night_to_day" => throw new NotImplementedException(),
		"api_req_sortie/ld_shooting" => throw new NotSupportedException(),
		"api_req_combined_battle/battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleBattleRequest>(file.Content),
		"api_req_combined_battle/sp_midnight" => JsonSerializer.Deserialize<ApiReqCombinedBattleSpMidnightRequest>(file.Content),
		"api_req_combined_battle/airbattle" => throw new NotImplementedException(),
		"api_req_combined_battle/battle_water" => JsonSerializer.Deserialize<ApiReqCombinedBattleBattleWaterRequest>(file.Content),
		"api_req_combined_battle/ld_airbattle" => JsonSerializer.Deserialize<ApiReqCombinedBattleLdAirbattleRequest>(file.Content),
		"api_req_combined_battle/ec_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleEcBattleRequest>(file.Content),
		"api_req_combined_battle/ec_night_to_day" => throw new NotImplementedException(),
		"api_req_combined_battle/each_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleEachBattleRequest>(file.Content),
		"api_req_combined_battle/each_battle_water" => JsonSerializer.Deserialize<ApiReqCombinedBattleEachBattleWaterRequest>(file.Content),
		"api_req_combined_battle/ld_shooting" => throw new NotImplementedException(),

		"api_req_battle_midnight/battle" => JsonSerializer.Deserialize<ApiReqBattleMidnightBattleRequest>(file.Content),
		"api_req_combined_battle/midnight_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleMidnightBattleRequest>(file.Content),
		"api_req_combined_battle/ec_midnight_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleEcMidnightBattleRequest>(file.Content),

		_ => throw new NotImplementedException(),
	};

	private static object? GetApiData<T>(this ApiResponse<T>? response) where T : class, new()
	{
		if (response?.ApiResult is not 1) return null;

		return response.ApiData;
	}

	private static object? GetApiData<T>(this ApiResponseList<T>? response) where T : class, new()
	{
		if (response?.ApiResult is not 1) return null;

		return response.ApiData;
	}

	private static object? GetApiData<T>(this ApiResponseNull<T>? response) where T : class, new()
	{
		if (response?.ApiResult is not 1) return null;

		return response.ApiData;
	}

	public static object? GetResponseApiData(this ApiFile file) => file.Name switch
	{
		"api_req_practice/battle" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqPracticeBattleResponse>>(file.Content).GetApiData(),

		"api_dmm_payment/paycheck" => JsonSerializer.Deserialize<ApiResponse<ApiDmmPaymentPaycheckResponse>>(file.Content).GetApiData(),
		"api_get_member/basic" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberBasicResponse>>(file.Content).GetApiData(),
		"api_get_member/deck" => JsonSerializer.Deserialize<ApiResponseList<FleetDataDto>>(file.Content).GetApiData(),
		"api_get_member/kdock" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberKdockResponse>>(file.Content).GetApiData(),
		"api_get_member/mapinfo" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberMapinfoResponse>>(file.Content).GetApiData(),
		"api_get_member/material" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberMaterialResponse>>(file.Content).GetApiData(),
		"api_get_member/mission" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberMissionResponse>>(file.Content).GetApiData(),
		"api_get_member/ndock" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberNdockResponse>>(file.Content).GetApiData(),
		"api_get_member/payitem" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberPayitemResponse>>(file.Content).GetApiData(),
		"api_get_member/picture_book" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPictureBookResponse>>(file.Content).GetApiData(),
		"api_get_member/practice" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPracticeResponse>>(file.Content).GetApiData(),
		"api_get_member/preset_deck" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPresetDeckResponse>>(file.Content).GetApiData(),
		"api_get_member/preset_slot" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPresetSlotResponse>>(file.Content).GetApiData(),
		"api_get_member/record" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberRecordResponse>>(file.Content).GetApiData(),
		"api_get_member/ship_deck" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberShipDeckResponse>>(file.Content).GetApiData(),
		"api_get_member/sortie_conditions" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberSortieConditionsResponse>>(file.Content).GetApiData(),
		"api_get_member/useitem" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberUseitemResponse>>(file.Content).GetApiData(),
		"api_port/port" => JsonSerializer.Deserialize<ApiResponse<ApiPortPortResponse>>(file.Content).GetApiData(),
		"api_req_air_corps/change_deployment_base" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsChangeDeploymentBaseResponse>>(file.Content).GetApiData(),
		"api_req_air_corps/set_action" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsSetActionResponse>>(file.Content).GetApiData(),
		"api_req_air_corps/set_plane" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsSetPlaneResponse>>(file.Content).GetApiData(),
		"api_req_air_corps/supply" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsSupplyResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleresultResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/goback_port" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleGobackPortResponse>>(file.Content).GetApiData(),
		"api_req_furniture/buy" => JsonSerializer.Deserialize<ApiResponse<ApiReqFurnitureBuyResponse>>(file.Content).GetApiData(),
		"api_req_hensei/change" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiChangeResponse>>(file.Content).GetApiData(),
		"api_req_hensei/combined" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiCombinedResponse>>(file.Content).GetApiData(),
		"api_req_hensei/lock" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiLockResponse>>(file.Content).GetApiData(),
		"api_req_hensei/preset_register" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiPresetRegisterResponse>>(file.Content).GetApiData(),
		"api_req_hensei/preset_select" => JsonSerializer.Deserialize<ApiResponse<FleetDataDto>>(file.Content).GetApiData(),
		"api_req_hokyu/charge" => JsonSerializer.Deserialize<ApiResponse<ApiReqHokyuChargeResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/can_preset_slot_select" => JsonSerializer.Deserialize<ApiResponseNull<APIReqKaisouCanPresetSlotSelectResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/lock" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouLockResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/marriage" => JsonSerializer.Deserialize<ApiResponse<ApiShip>>(file.Content).GetApiData(),
		"api_req_kaisou/open_exslot" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouOpenExslotResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/powerup" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouPowerupResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/preset_slot_register" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqKaisouPresetSlotRegisterResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/preset_slot_select" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqKaisouPresetSlotSelectResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/remodeling" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouRemodelingResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/slot_deprive" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotDepriveResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/slot_exchange_index" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotExchangeIndexResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/slotset" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotsetResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/slotset_ex" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotsetExResponse>>(file.Content).GetApiData(),
		"api_req_kaisou/unsetslot_all" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouUnsetslotAllResponse>>(file.Content).GetApiData(),
		"api_req_kousyou/createitem" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouCreateitemResponse>>(file.Content).GetApiData(),
		"api_req_kousyou/createship" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouCreateshipResponse>>(file.Content).GetApiData(),
		"api_req_kousyou/createship_speedchange" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouCreateshipSpeedchangeResponse>>(file.Content).GetApiData(),
		"api_req_kousyou/destroyitem2" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouDestroyitem2Response>>(file.Content).GetApiData(),
		"api_req_kousyou/destroyship" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouDestroyshipResponse>>(file.Content).GetApiData(),
		"api_req_kousyou/getship" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouGetshipResponse>>(file.Content).GetApiData(),
		"api_req_kousyou/remodel_slot" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouRemodelSlotResponse>>(file.Content).GetApiData(),
		"api_req_kousyou/remodel_slotlist" => JsonSerializer.Deserialize<ApiResponseList<APIReqKousyouRemodelSlotlistResponse>>(file.Content).GetApiData(),
		"api_req_kousyou/remodel_slotlist_detail" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouRemodelSlotlistDetailResponse>>(file.Content).GetApiData(),
		"api_req_map/next" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapNextResponse>>(file.Content).GetApiData(),
		"api_req_map/select_eventmap_rank" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapSelectEventmapRankResponse>>(file.Content).GetApiData(),
		"api_req_map/start" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapStartResponse>>(file.Content).GetApiData(),
		"api_req_map/start_air_base" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapStartAirBaseResponse>>(file.Content).GetApiData(),
		"api_req_map/anchorage_repair" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapAnchorageRepairResponse>>(file.Content).GetApiData(),
		"api_req_member/get_event_selected_reward" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberGetEventSelectedRewardResponse>>(file.Content).GetApiData(),
		"api_req_member/get_incentive" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberGetIncentiveResponse>>(file.Content).GetApiData(),
		"api_req_member/get_practice_enemyinfo" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberGetPracticeEnemyinfoResponse>>(file.Content).GetApiData(),
		"api_req_member/itemuse" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberItemuseResponse>>(file.Content).GetApiData(),
		"api_req_member/payitemuse" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberPayitemuseResponse>>(file.Content).GetApiData(),
		"api_req_member/set_flagship_position" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberSetFlagshipPositionResponse>>(file.Content).GetApiData(),
		"api_req_member/set_friendly_request" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqMemberSetFriendlyRequestResponse>>(file.Content).GetApiData(),
		"api_req_member/set_oss_condition" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberSetOssConditionResponse>>(file.Content).GetApiData(),
		"api_req_member/updatecomment" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqMemberUpdatecommentResponse>>(file.Content).GetApiData(),
		"api_req_member/updatedeckname" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqMemberUpdatedecknameResponse>>(file.Content).GetApiData(),
		"api_req_mission/result" => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionResultResponse>>(file.Content).GetApiData(),
		"api_req_mission/return_instruction" => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionReturnInstructionResponse>>(file.Content).GetApiData(),
		"api_req_mission/start" => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionStartResponse>>(file.Content).GetApiData(),
		"api_req_nyukyo/speedchange" => JsonSerializer.Deserialize<ApiResponse<ApiReqNyukyoSpeedchangeResponse>>(file.Content).GetApiData(),
		"api_req_nyukyo/start" => JsonSerializer.Deserialize<ApiResponseNull<APIReqNyukyoStartResponse>>(file.Content).GetApiData(),
		"api_req_practice/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeBattleResponse>>(file.Content).GetApiData(),
		"api_req_practice/battle_result" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeBattleResultResponse>>(file.Content).GetApiData(),
		"api_req_practice/change_matching_kind" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeChangeMatchingKindResponse>>(file.Content).GetApiData(),
		"api_req_practice/midnight_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeMidnightBattleResponse>>(file.Content).GetApiData(),
		"api_req_quest/clearitemget" => JsonSerializer.Deserialize<ApiResponse<ApiReqQuestClearitemgetResponse>>(file.Content).GetApiData(),
		"api_req_quest/start" => JsonSerializer.Deserialize<ApiResponse<ApiReqQuestStartResponse>>(file.Content).GetApiData(),
		"api_req_quest/stop" => JsonSerializer.Deserialize<ApiResponse<ApiReqQuestStopResponse>>(file.Content).GetApiData(),
		"api_req_ranking/mxltvkpyuklh" => JsonSerializer.Deserialize<ApiResponse<ApiReqRankingMxltvkpyuklhResponse>>(file.Content).GetApiData(),
		"api_req_sortie/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieBattleresultResponse>>(file.Content).GetApiData(),
		"api_req_sortie/goback_port" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqSortieGobackPortResponse>>(file.Content).GetApiData(),

		_ => file.GetBattleResponseApiData(),
	};

	public static object? GetBattleResponseApiData(this ApiFile file) => file.Name switch
	{
		"api_req_sortie/battle" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqSortieBattleResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/battle" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqCombinedBattleBattleResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/battle_water" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqCombinedBattleBattleWaterResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/ec_battle" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqCombinedBattleEcBattleResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/each_battle" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqCombinedBattleEachBattleResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/each_battle_water" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqCombinedBattleEachBattleWaterResponse>>(file.Content).GetApiData(),

		"api_req_sortie/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieBattleResponse>>(file.Content).GetApiData(),
		"api_req_battle_midnight/sp_midnight" => JsonSerializer.Deserialize<ApiResponse<ApiReqBattleMidnightSpMidnightResponse>>(file.Content).GetApiData(),
		"api_req_sortie/airbattle" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieAirbattleResponse>>(file.Content).GetApiData(),
		"api_req_sortie/ld_airbattle" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieLdAirbattleResponse>>(file.Content).GetApiData(),
		"api_req_sortie/night_to_day" => throw new NotImplementedException(),
		"api_req_sortie/ld_shooting" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieLdShootingResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/sp_midnight" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleSpMidnightResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/airbattle" => throw new NotImplementedException(),
		"api_req_combined_battle/battle_water" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleWaterResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/ld_airbattle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleLdAirbattleResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/ec_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEcBattleResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/ec_night_to_day" => throw new NotImplementedException(),
		"api_req_combined_battle/each_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEachBattleResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/each_battle_water" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEachBattleWaterResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/ld_shooting" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleLdShootingResponse>>(file.Content).GetApiData(),

		"api_req_battle_midnight/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqBattleMidnightBattleResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/midnight_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleMidnightBattleResponse>>(file.Content).GetApiData(),
		"api_req_combined_battle/ec_midnight_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEcMidnightBattleResponse>>(file.Content).GetApiData(),

		_ => throw new NotImplementedException(),
	};

	// normal battle - day
	// night node - battle
	// night to day - night
	// etc...
	public static bool IsFirstBattleApi(this ApiFile apiFile) => apiFile.Name is
		"api_req_sortie/battle" or // normal day
		"api_req_battle_midnight/sp_midnight" or // night node
		"api_req_sortie/airbattle" or // single air raid
		"api_req_sortie/ld_airbattle" or // single air raid
		"api_req_sortie/night_to_day" or // single night to day
		"api_req_sortie/ld_shooting" or // single fleet radar ambush
		"api_req_combined_battle/battle" or // combined normal
		"api_req_combined_battle/sp_midnight" or // combined night battle
		"api_req_combined_battle/airbattle" or // combined air exchange ?
		"api_req_combined_battle/battle_water" or // CTF TCF combined battle
		"api_req_combined_battle/ld_airbattle" or // air raid
		"api_req_combined_battle/ec_battle" or // CTF enemy combined battle
		"api_req_combined_battle/ec_night_to_day" or // enemy combined night to day
		"api_req_combined_battle/each_battle" or // STF combined vs combined
		"api_req_combined_battle/each_battle_water" or // STF combined
		"api_req_combined_battle/ld_shooting"; // combined radar ambush

	// normal battle - night
	// night to day - day
	// etc...
	public static bool IsSecondBattleApi(this ApiFile apiFile) => apiFile.Name is
		"api_req_battle_midnight/battle" or // normal night
		"api_req_combined_battle/midnight_battle" or // combined day to night
		"api_req_combined_battle/ec_midnight_battle"; // combined normal night battle

	public static bool IsBattleEndApi(this ApiFile apiFile) => apiFile.Name is
		"api_req_sortie/battleresult" or
		"api_req_combined_battle/battleresult" or
		"api_req_practice/battle_result";

	public static bool IsMapProgressApi(this ApiFile apiFile) => apiFile.Name is
		"api_req_map/start" or
		"api_req_map/next";

	public static async Task<List<SortieRecord>> WithApiFiles(this IEnumerable<SortieRecord> sorties,
		ElectronicObserverContext db, CancellationToken cancellationToken = default)
	{
		List<SortieRecord> sortieRecords = sorties.ToList();

		List<int> idsToLoad = sortieRecords
			.Where(s => s.ApiFiles.Count is 0)
			.Select(s => s.Id)
			.ToList();

		IEnumerable<SortieRecord> loadedSorties = sortieRecords.Where(s => s.ApiFiles.Count is not 0);

		List<SortieRecord> unloadedSorties = idsToLoad switch
		{
			{ Count: 0 } => [],

			_ => await db.Sorties
				.Include(s => s.ApiFiles)
				.Where(s => idsToLoad.Contains(s.Id))
				.ToListAsync(cancellationToken),
		};

		return [.. unloadedSorties, .. loadedSorties];
	}

	public static async Task EnsureApiFilesLoaded(this SortieRecord sortie, ElectronicObserverContext db, CancellationToken cancellationToken = default)
	{
		if (sortie.ApiFiles.Count is not 0) return;

		sortie.ApiFiles = await db.ApiFiles
			.AsNoTracking()
			.Where(f => f.SortieRecordId == sortie.Id)
			.ToListAsync(cancellationToken);
	}

	/// <summary>
	/// Removes api_token values.
	/// </summary>
	public static void CleanRequests(this SortieRecord sortie)
	{
		foreach (ApiFile apiFile in sortie.ApiFiles)
		{
			CleanRequest(apiFile);
		}
	}

	/// <summary>
	/// Removes api_token values.
	/// </summary>
	public static ApiFile CleanRequest(this ApiFile apiFile)
	{
		if (apiFile.ApiFileType is not ApiFileType.Request) return apiFile;

		Dictionary<string, string>? requestData = JsonSerializer
			.Deserialize<Dictionary<string, string>>(apiFile.Content);

		if (requestData is null) return apiFile;

		requestData.Remove("api_token");

		apiFile.Content = JsonSerializer.Serialize(requestData);

		return apiFile;
	}

	public static async Task<int?> GetAdmiralLevel(this SortieRecord sortieRecord, ElectronicObserverContext db, CancellationToken cancellationToken = default)
	{
		if (sortieRecord.ApiFiles.Count is 0) return null;

		return await GetAdmiralLevel(sortieRecord.ApiFiles.First().TimeStamp, db, cancellationToken);
	}

	public static async Task<int?> GetAdmiralLevel(this ExpeditionRecord expeditionRecord, ElectronicObserverContext db, CancellationToken cancellationToken = default)
	{
		if (expeditionRecord.ApiFiles.Count is 0) return null;

		return await GetAdmiralLevel(expeditionRecord.ApiFiles.First().TimeStamp, db, cancellationToken);
	}

	private static async Task<int?> GetAdmiralLevel(DateTime timeStamp, ElectronicObserverContext db, CancellationToken cancellationToken = default)
	{
		// get the last port response right before the time stamp
		ApiFile? portFile = await db.ApiFiles
			.Where(f => f.ApiFileType == ApiFileType.Response)
			.Where(f => f.Name == "api_port/port")
			.Where(f => f.TimeStamp < timeStamp)
			.OrderByDescending(f => f.TimeStamp)
			.FirstOrDefaultAsync(cancellationToken);

		if (portFile is null) return null;

		try
		{
			ApiPortPortResponse? port = JsonSerializer
				.Deserialize<ApiResponse<ApiPortPortResponse>>(portFile.Content)?.ApiData;

			if (port != null)
			{
				return port.ApiBasic.ApiLevel;
			}
		}
		catch
		{
			return null;
		}

		return null;
	}
}
