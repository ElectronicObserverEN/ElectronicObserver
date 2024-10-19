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
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Questlist;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Record;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship2;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.SortieConditions;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Useitem;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.ChangeDeploymentBase;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.ExpandBase;
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
using ElectronicObserver.KancolleApi.Types.ApiReqFurniture.Change;
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
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotDelete;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotRegister;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotSelect;
using ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotUpdateExslotFlag;
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

	public static object? GetRequestApiData(this ApiFile file, JsonSerializerOptions? options = null) => file.Name switch
	{
		"api_dmm_payment/paycheck" => JsonSerializer.Deserialize<ApiDmmPaymentPaycheckRequest>(file.Content, options),
		
		"api_get_member/basic" => JsonSerializer.Deserialize<ApiGetMemberBasicRequest>(file.Content, options),
		"api_get_member/deck" => JsonSerializer.Deserialize<ApiGetMemberDeckRequest>(file.Content, options),
		"api_get_member/kdock" => JsonSerializer.Deserialize<ApiGetMemberKdockRequest>(file.Content, options),
		"api_get_member/mapinfo" => JsonSerializer.Deserialize<ApiGetMemberMapinfoRequest>(file.Content, options),
		"api_get_member/material" => JsonSerializer.Deserialize<ApiGetMemberMaterialRequest>(file.Content, options),
		"api_get_member/mission" => JsonSerializer.Deserialize<ApiGetMemberMissionRequest>(file.Content, options),
		"api_get_member/ndock" => JsonSerializer.Deserialize<ApiGetMemberNdockRequest>(file.Content, options),
		"api_get_member/payitem" => JsonSerializer.Deserialize<ApiGetMemberPayitemRequest>(file.Content, options),
		"api_get_member/picture_book" => JsonSerializer.Deserialize<ApiGetMemberPictureBookRequest>(file.Content, options),
		"api_get_member/practice" => JsonSerializer.Deserialize<ApiGetMemberPracticeRequest>(file.Content, options),
		"api_get_member/preset_deck" => JsonSerializer.Deserialize<ApiGetMemberPresetDeckRequest>(file.Content, options),
		"api_get_member/preset_slot" => JsonSerializer.Deserialize<ApiGetMemberPresetSlotRequest>(file.Content, options),
		"api_get_member/questlist" => JsonSerializer.Deserialize<ApiGetMemberQuestlistRequest>(file.Content, options),
		"api_get_member/record" => JsonSerializer.Deserialize<ApiGetMemberRecordRequest>(file.Content, options),
		"api_get_member/ship_deck" => JsonSerializer.Deserialize<ApiGetMemberShipDeckRequest>(file.Content, options),
		"api_get_member/sortie_conditions" => JsonSerializer.Deserialize<ApiGetMemberSortieConditionsRequest>(file.Content, options),
		"api_get_member/useitem" => JsonSerializer.Deserialize<ApiGetMemberUseitemRequest>(file.Content, options),
		"api_get_member/ship2" => JsonSerializer.Deserialize<ApiGetMemberShip2Request>(file.Content, options),
		
		"api_port/port" => JsonSerializer.Deserialize<ApiPortPortRequest>(file.Content, options),
		"api_req_air_corps/change_deployment_base" => JsonSerializer.Deserialize<ApiReqAirCorpsChangeDeploymentBaseRequest>(file.Content, options),
		"api_req_air_corps/set_action" => JsonSerializer.Deserialize<ApiReqAirCorpsSetActionRequest>(file.Content, options),
		"api_req_air_corps/set_plane" => JsonSerializer.Deserialize<ApiReqAirCorpsSetPlaneRequest>(file.Content, options),
		"api_req_air_corps/supply" => JsonSerializer.Deserialize<ApiReqAirCorpsSupplyRequest>(file.Content, options),
		"api_req_air_corps/expand_base" => JsonSerializer.Deserialize<ApiReqAirCorpsExpandBaseRequest>(file.Content, options),
		"api_req_combined_battle/battleresult" => JsonSerializer.Deserialize<ApiReqCombinedBattleBattleresultRequest>(file.Content, options),
		"api_req_combined_battle/goback_port" => JsonSerializer.Deserialize<ApiReqCombinedBattleGobackPortRequest>(file.Content, options),
		"api_req_furniture/buy" => JsonSerializer.Deserialize<ApiReqFurnitureBuyRequest>(file.Content, options),
		"api_req_furniture/change" => JsonSerializer.Deserialize<ApiReqFurnitureChangeRequest>(file.Content, options),
		"api_req_hensei/change" => JsonSerializer.Deserialize<ApiReqHenseiChangeRequest>(file.Content, options),
		"api_req_hensei/combined" => JsonSerializer.Deserialize<ApiReqHenseiCombinedRequest>(file.Content, options),
		"api_req_hensei/lock" => JsonSerializer.Deserialize<ApiReqHenseiLockRequest>(file.Content, options),
		"api_req_hensei/preset_register" => JsonSerializer.Deserialize<ApiReqHenseiPresetRegisterRequest>(file.Content, options),
		"api_req_hensei/preset_select" => JsonSerializer.Deserialize<ApiReqHenseiPresetSelectRequest>(file.Content, options),
		"api_req_hokyu/charge" => JsonSerializer.Deserialize<ApiReqHokyuChargeRequest>(file.Content, options),
		"api_req_kaisou/can_preset_slot_select" => JsonSerializer.Deserialize<ApiReqKaisouCanPresetSlotSelectRequest>(file.Content, options),
		"api_req_kaisou/lock" => JsonSerializer.Deserialize<ApiReqKaisouLockRequest>(file.Content, options),
		"api_req_kaisou/marriage" => JsonSerializer.Deserialize<ApiReqKaisouMarriageRequest>(file.Content, options),
		"api_req_kaisou/open_exslot" => JsonSerializer.Deserialize<ApiReqKaisouOpenExslotRequest>(file.Content, options),
		"api_req_kaisou/powerup" => JsonSerializer.Deserialize<ApiReqKaisouPowerupRequest>(file.Content, options),
		"api_req_kaisou/preset_slot_register" => JsonSerializer.Deserialize<ApiReqKaisouPresetSlotRegisterRequest>(file.Content, options),
		"api_req_kaisou/preset_slot_select" => JsonSerializer.Deserialize<ApiReqKaisouPresetSlotSelectRequest>(file.Content, options),
		"api_req_kaisou/preset_slot_update_exslot_flag" => JsonSerializer.Deserialize<ApiReqKaisouPresetSlotUpdateExslotFlagRequest>(file.Content, options),
		"api_req_kaisou/preset_slot_delete" => JsonSerializer.Deserialize<ApiReqKaisouPresetSlotDeleteRequest>(file.Content, options),
		"api_req_kaisou/remodeling" => JsonSerializer.Deserialize<ApiReqKaisouRemodelingRequest>(file.Content, options),
		"api_req_kaisou/slot_deprive" => JsonSerializer.Deserialize<ApiReqKaisouSlotDepriveRequest>(file.Content, options),
		"api_req_kaisou/slot_exchange_index" => JsonSerializer.Deserialize<ApiReqKaisouSlotExchangeIndexRequest>(file.Content, options),
		"api_req_kaisou/slotset" => JsonSerializer.Deserialize<ApiReqKaisouSlotsetRequest>(file.Content, options),
		"api_req_kaisou/slotset_ex" => JsonSerializer.Deserialize<ApiReqKaisouSlotsetExRequest>(file.Content, options),
		"api_req_kaisou/unsetslot_all" => JsonSerializer.Deserialize<ApiReqKaisouUnsetslotAllRequest>(file.Content, options),
		"api_req_kousyou/createitem" => JsonSerializer.Deserialize<ApiReqKousyouCreateitemRequest>(file.Content, options),
		"api_req_kousyou/createship" => JsonSerializer.Deserialize<ApiReqKousyouCreateshipRequest>(file.Content, options),
		"api_req_kousyou/createship_speedchange" => JsonSerializer.Deserialize<ApiReqKousyouCreateshipSpeedchangeRequest>(file.Content, options),
		"api_req_kousyou/destroyitem2" => JsonSerializer.Deserialize<ApiReqKousyouDestroyitem2Request>(file.Content, options),
		"api_req_kousyou/destroyship" => JsonSerializer.Deserialize<ApiReqKousyouDestroyshipRequest>(file.Content, options),
		"api_req_kousyou/getship" => JsonSerializer.Deserialize<ApiReqKousyouGetshipRequest>(file.Content, options),
		"api_req_kousyou/remodel_slot" => JsonSerializer.Deserialize<ApiReqKousyouRemodelSlotRequest>(file.Content, options),
		"api_req_kousyou/remodel_slotlist" => JsonSerializer.Deserialize<ApiReqKousyouRemodelSlotlistRequest>(file.Content, options),
		"api_req_kousyou/remodel_slotlist_detail" => JsonSerializer.Deserialize<ApiReqKousyouRemodelSlotlistDetailRequest>(file.Content, options),
		"api_req_map/next" => JsonSerializer.Deserialize<ApiReqMapNextRequest>(file.Content, options),
		"api_req_map/select_eventmap_rank" => JsonSerializer.Deserialize<ApiReqMapSelectEventmapRankRequest>(file.Content, options),
		"api_req_map/start" => JsonSerializer.Deserialize<ApiReqMapStartRequest>(file.Content, options),
		"api_req_map/start_air_base" => JsonSerializer.Deserialize<ApiReqMapStartAirBaseRequest>(file.Content, options),
		"api_req_member/get_event_selected_reward" => JsonSerializer.Deserialize<ApiReqMemberGetEventSelectedRewardRequest>(file.Content, options),
		"api_req_member/get_incentive" => JsonSerializer.Deserialize<ApiReqMemberGetIncentiveRequest>(file.Content, options),
		"api_req_member/get_practice_enemyinfo" => JsonSerializer.Deserialize<ApiReqMemberGetPracticeEnemyinfoRequest>(file.Content, options),
		"api_req_member/itemuse" => JsonSerializer.Deserialize<ApiReqMemberItemuseRequest>(file.Content, options),
		"api_req_member/payitemuse" => JsonSerializer.Deserialize<ApiReqMemberPayitemuseRequest>(file.Content, options),
		"api_req_member/set_flagship_position" => JsonSerializer.Deserialize<ApiReqMemberSetFlagshipPositionRequest>(file.Content, options),
		"api_req_member/set_friendly_request" => JsonSerializer.Deserialize<ApiReqMemberSetFriendlyRequestRequest>(file.Content, options),
		"api_req_member/set_oss_condition" => JsonSerializer.Deserialize<ApiReqMemberSetOssConditionRequest>(file.Content, options),
		"api_req_member/updatecomment" => JsonSerializer.Deserialize<ApiReqMemberUpdatecommentRequest>(file.Content, options),
		"api_req_member/updatedeckname" => JsonSerializer.Deserialize<ApiReqMemberUpdatedecknameRequest>(file.Content, options),
		"api_req_mission/result" => JsonSerializer.Deserialize<ApiReqMissionResultRequest>(file.Content, options),
		"api_req_mission/return_instruction" => JsonSerializer.Deserialize<ApiReqMissionReturnInstructionRequest>(file.Content, options),
		"api_req_mission/start" => JsonSerializer.Deserialize<ApiReqMissionStartRequest>(file.Content, options),
		"api_req_nyukyo/speedchange" => JsonSerializer.Deserialize<ApiReqNyukyoSpeedchangeRequest>(file.Content, options),
		"api_req_nyukyo/start" => JsonSerializer.Deserialize<ApiReqNyukyoStartRequest>(file.Content, options),
		"api_req_practice/battle" => JsonSerializer.Deserialize<ApiReqPracticeBattleRequest>(file.Content, options),
		"api_req_practice/battle_result" => JsonSerializer.Deserialize<ApiReqPracticeBattleResultRequest>(file.Content, options),
		"api_req_practice/change_matching_kind" => JsonSerializer.Deserialize<ApiReqPracticeChangeMatchingKindRequest>(file.Content, options),
		"api_req_practice/midnight_battle" => JsonSerializer.Deserialize<ApiReqPracticeMidnightBattleRequest>(file.Content, options),
		"api_req_quest/clearitemget" => JsonSerializer.Deserialize<ApiReqQuestClearitemgetRequest>(file.Content, options),
		"api_req_quest/start" => JsonSerializer.Deserialize<ApiReqQuestStartRequest>(file.Content, options),
		"api_req_quest/stop" => JsonSerializer.Deserialize<ApiReqQuestStopRequest>(file.Content, options),
		"api_req_ranking/mxltvkpyuklh" => JsonSerializer.Deserialize<ApiReqRankingMxltvkpyuklhRequest>(file.Content, options),
		"api_req_sortie/battleresult" => JsonSerializer.Deserialize<ApiReqSortieBattleresultRequest>(file.Content, options),
		"api_req_sortie/goback_port" => JsonSerializer.Deserialize<ApiReqSortieGobackPortRequest>(file.Content, options),

		"api_req_sortie/battle" => JsonSerializer.Deserialize<ApiReqSortieBattleRequest>(file.Content, options),
		"api_req_battle_midnight/sp_midnight" => JsonSerializer.Deserialize<ApiReqBattleMidnightSpMidnightRequest>(file.Content, options),
		"api_req_sortie/airbattle" => JsonSerializer.Deserialize<ApiReqSortieAirbattleRequest>(file.Content, options),
		"api_req_sortie/ld_airbattle" => JsonSerializer.Deserialize<ApiReqSortieLdAirbattleRequest>(file.Content, options),
		"api_req_sortie/night_to_day" => throw new NotImplementedException(),
		"api_req_sortie/ld_shooting" => JsonSerializer.Deserialize<ApiReqSortieLdShootingRequest>(file.Content, options),
		"api_req_combined_battle/battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleBattleRequest>(file.Content, options),
		"api_req_combined_battle/sp_midnight" => JsonSerializer.Deserialize<ApiReqCombinedBattleSpMidnightRequest>(file.Content, options),
		"api_req_combined_battle/airbattle" => throw new NotImplementedException(),
		"api_req_combined_battle/battle_water" => JsonSerializer.Deserialize<ApiReqCombinedBattleBattleWaterRequest>(file.Content, options),
		"api_req_combined_battle/ld_airbattle" => JsonSerializer.Deserialize<ApiReqCombinedBattleLdAirbattleRequest>(file.Content, options),
		"api_req_combined_battle/ec_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleEcBattleRequest>(file.Content, options),
		"api_req_combined_battle/ec_night_to_day" => throw new NotImplementedException(),
		"api_req_combined_battle/each_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleEachBattleRequest>(file.Content, options),
		"api_req_combined_battle/each_battle_water" => JsonSerializer.Deserialize<ApiReqCombinedBattleEachBattleWaterRequest>(file.Content, options),
		"api_req_combined_battle/ld_shooting" => JsonSerializer.Deserialize<ApiReqCombinedBattleLdShootingRequest>(file.Content, options),

		"api_req_battle_midnight/battle" => JsonSerializer.Deserialize<ApiReqBattleMidnightBattleRequest>(file.Content, options),
		"api_req_combined_battle/midnight_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleMidnightBattleRequest>(file.Content, options),
		"api_req_combined_battle/ec_midnight_battle" => JsonSerializer.Deserialize<ApiReqCombinedBattleEcMidnightBattleRequest>(file.Content, options),

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

	public static object? GetResponseApiData(this ApiFile file, JsonSerializerOptions? options = null) => file.Name switch
	{
		"api_req_practice/battle" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqPracticeBattleResponse>>(file.Content, options).GetApiData(),

		"api_dmm_payment/paycheck" => JsonSerializer.Deserialize<ApiResponse<ApiDmmPaymentPaycheckResponse>>(file.Content, options).GetApiData(),
		
		"api_get_member/basic" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberBasicResponse>>(file.Content, options).GetApiData(),
		"api_get_member/deck" => JsonSerializer.Deserialize<ApiResponseList<FleetDataDto>>(file.Content, options).GetApiData(),
		"api_get_member/kdock" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberKdockResponse>>(file.Content, options).GetApiData(),
		"api_get_member/mapinfo" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberMapinfoResponse>>(file.Content, options).GetApiData(),
		"api_get_member/material" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberMaterialResponse>>(file.Content, options).GetApiData(),
		"api_get_member/mission" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberMissionResponse>>(file.Content, options).GetApiData(),
		"api_get_member/ndock" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberNdockResponse>>(file.Content, options).GetApiData(),
		"api_get_member/payitem" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberPayitemResponse>>(file.Content, options).GetApiData(),
		"api_get_member/picture_book" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPictureBookResponse>>(file.Content, options).GetApiData(),
		"api_get_member/practice" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPracticeResponse>>(file.Content, options).GetApiData(),
		"api_get_member/preset_deck" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPresetDeckResponse>>(file.Content, options).GetApiData(),
		"api_get_member/preset_slot" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberPresetSlotResponse>>(file.Content, options).GetApiData(),
		"api_get_member/questlist" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberQuestlistResponse>>(file.Content, options).GetApiData(),
		"api_get_member/record" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberRecordResponse>>(file.Content, options).GetApiData(),
		"api_get_member/ship_deck" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberShipDeckResponse>>(file.Content, options).GetApiData(),
		"api_get_member/sortie_conditions" => JsonSerializer.Deserialize<ApiResponse<ApiGetMemberSortieConditionsResponse>>(file.Content, options).GetApiData(),
		"api_get_member/useitem" => JsonSerializer.Deserialize<ApiResponseList<ApiGetMemberUseitemResponse>>(file.Content, options).GetApiData(),
		"api_get_member/ship2" => JsonSerializer.Deserialize<ApiShip2Response>(file.Content, options),
		
		"api_port/port" => JsonSerializer.Deserialize<ApiResponse<ApiPortPortResponse>>(file.Content, options).GetApiData(),
		"api_req_air_corps/change_deployment_base" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsChangeDeploymentBaseResponse>>(file.Content, options).GetApiData(),
		"api_req_air_corps/set_action" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsSetActionResponse>>(file.Content, options).GetApiData(),
		"api_req_air_corps/set_plane" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsSetPlaneResponse>>(file.Content, options).GetApiData(),
		"api_req_air_corps/supply" => JsonSerializer.Deserialize<ApiResponse<ApiReqAirCorpsSupplyResponse>>(file.Content, options).GetApiData(),
		"api_req_air_corps/expand_base" => JsonSerializer.Deserialize<ApiResponseList<ApiReqAirCorpsExpandBaseResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleresultResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/goback_port" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleGobackPortResponse>>(file.Content, options).GetApiData(),
		"api_req_furniture/buy" => JsonSerializer.Deserialize<ApiResponse<ApiReqFurnitureBuyResponse>>(file.Content, options).GetApiData(),
		"api_req_furniture/change" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqFurnitureChangeResponse>>(file.Content, options).GetApiData(),
		"api_req_hensei/change" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiChangeResponse>>(file.Content, options).GetApiData(),
		"api_req_hensei/combined" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiCombinedResponse>>(file.Content, options).GetApiData(),
		"api_req_hensei/lock" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiLockResponse>>(file.Content, options).GetApiData(),
		"api_req_hensei/preset_register" => JsonSerializer.Deserialize<ApiResponse<ApiReqHenseiPresetRegisterResponse>>(file.Content, options).GetApiData(),
		"api_req_hensei/preset_select" => JsonSerializer.Deserialize<ApiResponse<FleetDataDto>>(file.Content, options).GetApiData(),
		"api_req_hokyu/charge" => JsonSerializer.Deserialize<ApiResponse<ApiReqHokyuChargeResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/can_preset_slot_select" => JsonSerializer.Deserialize<ApiResponseNull<APIReqKaisouCanPresetSlotSelectResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/lock" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouLockResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/marriage" => JsonSerializer.Deserialize<ApiResponse<ApiShip>>(file.Content, options).GetApiData(),
		"api_req_kaisou/open_exslot" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouOpenExslotResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/powerup" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouPowerupResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/preset_slot_register" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqKaisouPresetSlotRegisterResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/preset_slot_select" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqKaisouPresetSlotSelectResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/preset_slot_update_exslot_flag" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqKaisouPresetSlotUpdateExslotFlagResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/preset_slot_delete" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqKaisouPresetSlotDeleteResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/remodeling" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouRemodelingResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/slot_deprive" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotDepriveResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/slot_exchange_index" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotExchangeIndexResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/slotset" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotsetResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/slotset_ex" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouSlotsetExResponse>>(file.Content, options).GetApiData(),
		"api_req_kaisou/unsetslot_all" => JsonSerializer.Deserialize<ApiResponse<ApiReqKaisouUnsetslotAllResponse>>(file.Content, options).GetApiData(),
		"api_req_kousyou/createitem" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouCreateitemResponse>>(file.Content, options).GetApiData(),
		"api_req_kousyou/createship" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouCreateshipResponse>>(file.Content, options).GetApiData(),
		"api_req_kousyou/createship_speedchange" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouCreateshipSpeedchangeResponse>>(file.Content, options).GetApiData(),
		"api_req_kousyou/destroyitem2" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouDestroyitem2Response>>(file.Content, options).GetApiData(),
		"api_req_kousyou/destroyship" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouDestroyshipResponse>>(file.Content, options).GetApiData(),
		"api_req_kousyou/getship" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouGetshipResponse>>(file.Content, options).GetApiData(),
		"api_req_kousyou/remodel_slot" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouRemodelSlotResponse>>(file.Content, options).GetApiData(),
		"api_req_kousyou/remodel_slotlist" => JsonSerializer.Deserialize<ApiResponseList<APIReqKousyouRemodelSlotlistResponse>>(file.Content, options).GetApiData(),
		"api_req_kousyou/remodel_slotlist_detail" => JsonSerializer.Deserialize<ApiResponse<ApiReqKousyouRemodelSlotlistDetailResponse>>(file.Content, options).GetApiData(),
		"api_req_map/next" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapNextResponse>>(file.Content, options).GetApiData(),
		"api_req_map/select_eventmap_rank" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapSelectEventmapRankResponse>>(file.Content, options).GetApiData(),
		"api_req_map/start" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapStartResponse>>(file.Content, options).GetApiData(),
		"api_req_map/start_air_base" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapStartAirBaseResponse>>(file.Content, options).GetApiData(),
		"api_req_map/anchorage_repair" => JsonSerializer.Deserialize<ApiResponse<ApiReqMapAnchorageRepairResponse>>(file.Content, options).GetApiData(),
		"api_req_member/get_event_selected_reward" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberGetEventSelectedRewardResponse>>(file.Content, options).GetApiData(),
		"api_req_member/get_incentive" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberGetIncentiveResponse>>(file.Content, options).GetApiData(),
		"api_req_member/get_practice_enemyinfo" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberGetPracticeEnemyinfoResponse>>(file.Content, options).GetApiData(),
		"api_req_member/itemuse" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberItemuseResponse>>(file.Content, options).GetApiData(),
		"api_req_member/payitemuse" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberPayitemuseResponse>>(file.Content, options).GetApiData(),
		"api_req_member/set_flagship_position" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberSetFlagshipPositionResponse>>(file.Content, options).GetApiData(),
		"api_req_member/set_friendly_request" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqMemberSetFriendlyRequestResponse>>(file.Content, options).GetApiData(),
		"api_req_member/set_oss_condition" => JsonSerializer.Deserialize<ApiResponse<ApiReqMemberSetOssConditionResponse>>(file.Content, options).GetApiData(),
		"api_req_member/updatecomment" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqMemberUpdatecommentResponse>>(file.Content, options).GetApiData(),
		"api_req_member/updatedeckname" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqMemberUpdatedecknameResponse>>(file.Content, options).GetApiData(),
		"api_req_mission/result" => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionResultResponse>>(file.Content, options).GetApiData(),
		"api_req_mission/return_instruction" => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionReturnInstructionResponse>>(file.Content, options).GetApiData(),
		"api_req_mission/start" => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionStartResponse>>(file.Content, options).GetApiData(),
		"api_req_nyukyo/speedchange" => JsonSerializer.Deserialize<ApiResponse<ApiReqNyukyoSpeedchangeResponse>>(file.Content, options).GetApiData(),
		"api_req_nyukyo/start" => JsonSerializer.Deserialize<ApiResponseNull<APIReqNyukyoStartResponse>>(file.Content, options).GetApiData(),
		"api_req_practice/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_practice/battle_result" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeBattleResultResponse>>(file.Content, options).GetApiData(),
		"api_req_practice/change_matching_kind" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeChangeMatchingKindResponse>>(file.Content, options).GetApiData(),
		"api_req_practice/midnight_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqPracticeMidnightBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_quest/clearitemget" => JsonSerializer.Deserialize<ApiResponse<ApiReqQuestClearitemgetResponse>>(file.Content, options).GetApiData(),
		"api_req_quest/start" => JsonSerializer.Deserialize<ApiResponse<ApiReqQuestStartResponse>>(file.Content, options).GetApiData(),
		"api_req_quest/stop" => JsonSerializer.Deserialize<ApiResponse<ApiReqQuestStopResponse>>(file.Content, options).GetApiData(),
		"api_req_ranking/mxltvkpyuklh" => JsonSerializer.Deserialize<ApiResponse<ApiReqRankingMxltvkpyuklhResponse>>(file.Content, options).GetApiData(),
		"api_req_sortie/battleresult" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieBattleresultResponse>>(file.Content, options).GetApiData(),
		"api_req_sortie/goback_port" => JsonSerializer.Deserialize<ApiResponseNull<ApiReqSortieGobackPortResponse>>(file.Content, options).GetApiData(),

		_ => file.GetBattleResponseApiData(options),
	};

	public static object? GetBattleResponseApiData(this ApiFile file, JsonSerializerOptions? options = null) => file.Name switch
	{
		"api_req_sortie/battle" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqSortieBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/battle" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqCombinedBattleBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/battle_water" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqCombinedBattleBattleWaterResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/ec_battle" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqCombinedBattleEcBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/each_battle" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqCombinedBattleEachBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/each_battle_water" when file.TimeStamp < OpeningTorpedoRework => JsonSerializer.Deserialize<ApiResponse<OpeningTorpedoRework_ApiReqCombinedBattleEachBattleWaterResponse>>(file.Content, options).GetApiData(),

		"api_req_sortie/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_battle_midnight/sp_midnight" => JsonSerializer.Deserialize<ApiResponse<ApiReqBattleMidnightSpMidnightResponse>>(file.Content, options).GetApiData(),
		"api_req_sortie/airbattle" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieAirbattleResponse>>(file.Content, options).GetApiData(),
		"api_req_sortie/ld_airbattle" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieLdAirbattleResponse>>(file.Content, options).GetApiData(),
		"api_req_sortie/night_to_day" => throw new NotImplementedException(),
		"api_req_sortie/ld_shooting" => JsonSerializer.Deserialize<ApiResponse<ApiReqSortieLdShootingResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/sp_midnight" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleSpMidnightResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/airbattle" => throw new NotImplementedException(),
		"api_req_combined_battle/battle_water" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleBattleWaterResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/ld_airbattle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleLdAirbattleResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/ec_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEcBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/ec_night_to_day" => throw new NotImplementedException(),
		"api_req_combined_battle/each_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEachBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/each_battle_water" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEachBattleWaterResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/ld_shooting" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleLdShootingResponse>>(file.Content, options).GetApiData(),

		"api_req_battle_midnight/battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqBattleMidnightBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/midnight_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleMidnightBattleResponse>>(file.Content, options).GetApiData(),
		"api_req_combined_battle/ec_midnight_battle" => JsonSerializer.Deserialize<ApiResponse<ApiReqCombinedBattleEcMidnightBattleResponse>>(file.Content, options).GetApiData(),

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

	private static Func<ElectronicObserverContext, List<int>, IAsyncEnumerable<SortieRecord>> SortieQuery { get; }
		= EF.CompileAsyncQuery((ElectronicObserverContext db, List<int> idsToLoad)
			=> db.Sorties
				.Include(s => s.ApiFiles)
				.Where(s => idsToLoad.Contains(s.Id)));

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

			_ => await SortieQuery(db, idsToLoad)
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
