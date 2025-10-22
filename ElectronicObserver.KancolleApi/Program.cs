using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiDmmPayment.Paycheck;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Basic;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.ChartAdditionalInfo;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Deck;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Furniture;
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
using ElectronicObserver.KancolleApi.Types.ApiGetMember.RequireInfo;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship2;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship3;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.SlotItem;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.SortieConditions;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Unsetslot;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Useitem;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.ChangeDeploymentBase;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.SetAction;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.SetPlane;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Supply;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Airbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.BattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcNightToDay;
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
using ElectronicObserver.KancolleApi.Types.ApiReqHensei.PresetDelete;
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
using ElectronicObserver.KancolleApi.Types.ApiReqMember.ItemuseCond;
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
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.NightToDay;
using ElectronicObserver.KancolleApi.Types.ApiStart2.GetData;
using ElectronicObserver.KancolleApi.Types.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseOpenApi();
	app.UseSwaggerUi();
}

RouteGroupBuilder dmmPayment = app.MapGroup("/api_dmm_payment").WithTags("api_dmm_payment");
dmmPayment.MapPost("/paycheck", (ApiDmmPaymentPaycheckRequest _) => new ApiResponse<ApiDmmPaymentPaycheckResponse>());

RouteGroupBuilder getMember = app.MapGroup("/api_get_member").WithTags("api_get_member");
getMember.MapPost("/basic", (ApiGetMemberBasicRequest _) => new ApiResponse<ApiGetMemberBasicResponse>());
getMember.MapPost("/chart_additional_info", (APIGetMemberChartAdditionalInfoRequest _) => new ApiResponseList<APIGetMemberChartAdditionalInfoResponse>());
getMember.MapPost("/deck", (ApiGetMemberDeckRequest _) => new ApiResponseList<FleetDataDto>());
getMember.MapPost("/furniture", (ApiGetMemberFurnitureRequest _) => new ApiGetMemberFurnitureResponse()); // todo - no idea if this even exists anymore
getMember.MapPost("/kdock", (ApiGetMemberKdockRequest _) => new ApiResponseList<ApiGetMemberKdockResponse>());
getMember.MapPost("/mapinfo", (ApiGetMemberMapinfoRequest _) => new ApiResponse<ApiGetMemberMapinfoResponse>());
getMember.MapPost("/material", (ApiGetMemberMaterialRequest _) => new ApiResponseList<ApiGetMemberMaterialResponse>());
getMember.MapPost("/mission", (ApiGetMemberMissionRequest _) => new ApiResponse<ApiGetMemberMissionResponse>());
getMember.MapPost("/ndock", (ApiGetMemberNdockRequest _) => new ApiResponseList<ApiGetMemberNdockResponse>());
getMember.MapPost("/payitem", (ApiGetMemberPayitemRequest _) => new ApiResponseList<ApiGetMemberPayitemResponse>());
getMember.MapPost("/picture_book", (ApiGetMemberPictureBookRequest _) => new ApiResponse<ApiGetMemberPictureBookResponse>());
getMember.MapPost("/practice", (ApiGetMemberPracticeRequest _) => new ApiResponse<ApiGetMemberPracticeResponse>());
getMember.MapPost("/preset_deck", (ApiGetMemberPresetDeckRequest _) => new ApiResponse<ApiGetMemberPresetDeckResponse>());
getMember.MapPost("/preset_slot", (ApiGetMemberPresetSlotRequest _) => new ApiResponse<ApiGetMemberPresetSlotResponse>());
getMember.MapPost("/questlist", (ApiGetMemberQuestlistRequest _) => new ApiResponse<ApiGetMemberQuestlistResponse>());
getMember.MapPost("/record", (ApiGetMemberRecordRequest _) => new ApiResponse<ApiGetMemberRecordResponse>());
getMember.MapPost("/require_info", (ApiGetMemberRequireInfoRequest _) => new ApiResponse<ApiGetMemberRequireInfoResponse>());
getMember.MapPost("/ship2", (ApiGetMemberShip2Request _) => new ApiShip2Response());
getMember.MapPost("/ship3", (ApiGetMemberShip3Request _) => new ApiResponse<ApiGetMemberShip3Response>());
getMember.MapPost("/ship_deck", (ApiGetMemberShipDeckRequest _) => new ApiResponse<ApiGetMemberShipDeckResponse>());
getMember.MapPost("/slot_item", (ApiGetMemberSlotItemRequest _) => new ApiResponseList<ApiGetMemberSlotItemResponse>());
getMember.MapPost("/sortie_conditions", (ApiGetMemberSortieConditionsRequest _) => new ApiResponse<ApiGetMemberSortieConditionsResponse>());
getMember.MapPost("/unsetslot", (ApiGetMemberUnsetslotRequest _) => new ApiResponse<ApiGetMemberUnsetslotResponse>());
getMember.MapPost("/useitem", (ApiGetMemberUseitemRequest _) => new ApiResponseList<ApiGetMemberUseitemResponse>());

RouteGroupBuilder port = app.MapGroup("/api_port").WithTags("api_port");
port.MapPost("/port", (ApiPortPortRequest _) => new ApiResponse<ApiPortPortResponse>());
port.MapPost("/airCorpsCondRecoveryWithTimer", (ApiPortAirCorpsCondRecoveryWithTimerRequest _) => new ApiResponse<ApiPortAirCorpsCondRecoveryWithTimerResponse>());

RouteGroupBuilder reqAirCorps = app.MapGroup("/api_req_air_corps").WithTags("api_req_air_corps");
reqAirCorps.MapPost("/change_deployment_base", (ApiReqAirCorpsChangeDeploymentBaseRequest _) => new ApiResponse<ApiReqAirCorpsChangeDeploymentBaseResponse>());
reqAirCorps.MapPost("/set_action", (ApiReqAirCorpsSetActionRequest _) => new ApiResponse<ApiReqAirCorpsSetActionResponse>());
reqAirCorps.MapPost("/set_plane", (ApiReqAirCorpsSetPlaneRequest _) => new ApiResponse<ApiReqAirCorpsSetPlaneResponse>());
reqAirCorps.MapPost("/supply", (ApiReqAirCorpsSupplyRequest _) => new ApiResponse<ApiReqAirCorpsSupplyResponse>());

RouteGroupBuilder reqBattleMidnight = app.MapGroup("/api_req_battle_midnight").WithTags("api_req_battle_midnight");
reqBattleMidnight.MapPost("/battle", (ApiReqBattleMidnightBattleRequest _) => new ApiResponse<ApiReqBattleMidnightBattleResponse>());
reqBattleMidnight.MapPost("/sp_midnight", (ApiReqBattleMidnightSpMidnightRequest _) => new ApiResponse<ApiReqBattleMidnightSpMidnightResponse>());

RouteGroupBuilder reqCombinedBattle = app.MapGroup("/api_req_combined_battle").WithTags("api_req_combined_battle");
reqCombinedBattle.MapPost("/airbattle", (ApiReqCombinedBattleAirbattleRequest _) => new ApiResponse<ApiReqCombinedBattleAirbattleResponse>());
reqCombinedBattle.MapPost("/battle", (ApiReqCombinedBattleBattleRequest _) => new ApiResponse<ApiReqCombinedBattleBattleResponse>());
reqCombinedBattle.MapPost("/battleresult", (ApiReqCombinedBattleBattleresultRequest _) => new ApiResponse<ApiReqCombinedBattleBattleresultResponse>());
reqCombinedBattle.MapPost("/battle_water", (ApiReqCombinedBattleBattleWaterRequest _) => new ApiResponse<ApiReqCombinedBattleBattleWaterResponse>());
reqCombinedBattle.MapPost("/each_battle", (ApiReqCombinedBattleEachBattleRequest _) => new ApiResponse<ApiReqCombinedBattleEachBattleResponse>());
reqCombinedBattle.MapPost("/each_battle_water", (ApiReqCombinedBattleEachBattleWaterRequest _) => new ApiResponse<ApiReqCombinedBattleEachBattleWaterResponse>());
reqCombinedBattle.MapPost("/ec_battle", (ApiReqCombinedBattleEcBattleRequest _) => new ApiResponse<ApiReqCombinedBattleEcBattleResponse>());
reqCombinedBattle.MapPost("/ec_midnight_battle", (ApiReqCombinedBattleEcMidnightBattleRequest _) => new ApiResponse<ApiReqCombinedBattleEcMidnightBattleResponse>());
reqCombinedBattle.MapPost("/ec_night_to_day", (ApiReqCombinedBattleEcNightToDayRequest _) => new ApiResponse<ApiReqCombinedBattleEcNightToDayResponse>());
reqCombinedBattle.MapPost("/goback_port", (ApiReqCombinedBattleGobackPortRequest _) => new ApiResponse<ApiReqCombinedBattleGobackPortResponse>());
reqCombinedBattle.MapPost("/ld_airbattle", (ApiReqCombinedBattleLdAirbattleRequest _) => new ApiResponse<ApiReqCombinedBattleLdAirbattleResponse>());
reqCombinedBattle.MapPost("/ld_shooting", (ApiReqCombinedBattleLdShootingRequest _) => new ApiResponse<ApiReqCombinedBattleLdShootingResponse>());
reqCombinedBattle.MapPost("/midnight_battle", (ApiReqCombinedBattleMidnightBattleRequest _) => new ApiResponse<ApiReqCombinedBattleMidnightBattleResponse>());
reqCombinedBattle.MapPost("/sp_midnight", (ApiReqCombinedBattleSpMidnightRequest _) => new ApiResponse<ApiReqCombinedBattleSpMidnightResponse>());

RouteGroupBuilder reqFurniture = app.MapGroup("/api_req_furniture").WithTags("api_req_furniture");
reqFurniture.MapPost("/buy", (ApiReqFurnitureBuyRequest _) => new ApiResponse<ApiReqFurnitureBuyResponse>());
reqFurniture.MapPost("/change", (ApiReqFurnitureChangeRequest _) => new ApiReqFurnitureChangeResponse());

RouteGroupBuilder reqHensei = app.MapGroup("/api_req_hensei").WithTags("api_req_hensei");
reqHensei.MapPost("/change", (ApiReqHenseiChangeRequest _) => new ApiResponse<ApiReqHenseiChangeResponse>());
reqHensei.MapPost("/combined", (ApiReqHenseiCombinedRequest _) => new ApiResponse<ApiReqHenseiCombinedResponse>());
reqHensei.MapPost("/lock", (ApiReqHenseiLockRequest _) => new ApiResponse<ApiReqHenseiLockResponse>());
reqHensei.MapPost("/preset_delete", (ApiReqHenseiPresetDeleteRequest _) => new ApiReqHenseiPresetDeleteResponse());
reqHensei.MapPost("/preset_register", (ApiReqHenseiPresetRegisterRequest _) => new ApiResponse<ApiReqHenseiPresetRegisterResponse>());
reqHensei.MapPost("/preset_select", (ApiReqHenseiPresetSelectRequest _) => new ApiResponse<FleetDataDto>());

RouteGroupBuilder reqHokyu = app.MapGroup("/api_req_hokyu").WithTags("api_req_hokyu");
reqHokyu.MapPost("/charge", (ApiReqHokyuChargeRequest _) => new ApiResponse<ApiReqHokyuChargeResponse>());

RouteGroupBuilder reqKaisou = app.MapGroup("/api_req_kaisou").WithTags("api_req_kaisou");
reqKaisou.MapPost("/can_preset_slot_select", (ApiReqKaisouCanPresetSlotSelectRequest _) => new ApiResponseNull<APIReqKaisouCanPresetSlotSelectResponse>());
reqKaisou.MapPost("/lock", (ApiReqKaisouLockRequest _) => new ApiResponse<ApiReqKaisouLockResponse>());
reqKaisou.MapPost("/marriage", (ApiReqKaisouMarriageRequest _) => new ApiResponse<ApiShip>());
reqKaisou.MapPost("/open_exslot", (ApiReqKaisouOpenExslotRequest _) => new ApiResponse<ApiReqKaisouOpenExslotResponse>());
reqKaisou.MapPost("/powerup", (ApiReqKaisouPowerupRequest _) => new ApiResponse<ApiReqKaisouPowerupResponse>());
reqKaisou.MapPost("/preset_slot_register", (ApiReqKaisouPresetSlotRegisterRequest _) => new ApiResponseNull<ApiReqKaisouPresetSlotRegisterResponse>());
reqKaisou.MapPost("/preset_slot_select", (ApiReqKaisouPresetSlotSelectRequest _) => new ApiResponseNull<ApiReqKaisouPresetSlotSelectResponse>());
reqKaisou.MapPost("/remodeling", (ApiReqKaisouRemodelingRequest _) => new ApiResponse<ApiReqKaisouRemodelingResponse>());
reqKaisou.MapPost("/slot_deprive", (ApiReqKaisouSlotDepriveRequest _) => new ApiResponse<ApiReqKaisouSlotDepriveResponse>());
reqKaisou.MapPost("/slot_exchange_index", (ApiReqKaisouSlotExchangeIndexRequest _) => new ApiResponse<ApiReqKaisouSlotExchangeIndexResponse>());
reqKaisou.MapPost("/slotset", (ApiReqKaisouSlotsetRequest _) => new ApiResponse<ApiReqKaisouSlotsetResponse>());
reqKaisou.MapPost("/slotset_ex", (ApiReqKaisouSlotsetExRequest _) => new ApiResponse<ApiReqKaisouSlotsetExResponse>());
reqKaisou.MapPost("/unsetslot_all", (ApiReqKaisouUnsetslotAllRequest _) => new ApiResponse<ApiReqKaisouUnsetslotAllResponse>());

RouteGroupBuilder reqKousyou = app.MapGroup("/api_req_kousyou").WithTags("api_req_kousyou");
reqKousyou.MapPost("/createitem", (ApiReqKousyouCreateitemRequest _) => new ApiResponse<ApiReqKousyouCreateitemResponse>());
reqKousyou.MapPost("/createship", (ApiReqKousyouCreateshipRequest _) => new ApiResponse<ApiReqKousyouCreateshipResponse>());
reqKousyou.MapPost("/createship_speedchange", (ApiReqKousyouCreateshipSpeedchangeRequest _) => new ApiResponse<ApiReqKousyouCreateshipSpeedchangeResponse>());
reqKousyou.MapPost("/destroyitem2", (ApiReqKousyouDestroyitem2Request _) => new ApiResponse<ApiReqKousyouDestroyitem2Response>());
reqKousyou.MapPost("/destroyship", (ApiReqKousyouDestroyshipRequest _) => new ApiResponse<ApiReqKousyouDestroyshipResponse>());
reqKousyou.MapPost("/getship", (ApiReqKousyouGetshipRequest _) => new ApiResponse<ApiReqKousyouGetshipResponse>());
reqKousyou.MapPost("/remodel_slot", (ApiReqKousyouRemodelSlotRequest _) => new ApiResponse<ApiReqKousyouRemodelSlotResponse>());
reqKousyou.MapPost("/remodel_slotlist", (ApiReqKousyouRemodelSlotlistRequest _) => new ApiResponseList<APIReqKousyouRemodelSlotlistResponse>());
reqKousyou.MapPost("/remodel_slotlist_detail", (ApiReqKousyouRemodelSlotlistDetailRequest _) => new ApiResponse<ApiReqKousyouRemodelSlotlistDetailResponse>());

RouteGroupBuilder reqMap = app.MapGroup("/api_req_map").WithTags("api_req_map");
reqMap.MapPost("/anchorage_repair", (ApiReqMapAnchorageRepairRequest _) => new ApiResponse<ApiReqMapAnchorageRepairResponse>());
reqMap.MapPost("/next", (ApiReqMapNextRequest _) => new ApiResponse<ApiReqMapNextResponse>());
reqMap.MapPost("/select_eventmap_rank", (ApiReqMapSelectEventmapRankRequest _) => new ApiResponse<ApiReqMapSelectEventmapRankResponse>());
reqMap.MapPost("/start", (ApiReqMapStartRequest _) => new ApiResponse<ApiReqMapStartResponse>());
reqMap.MapPost("/start_air_base", (ApiReqMapStartAirBaseRequest _) => new ApiResponse<ApiReqMapStartAirBaseResponse>());

RouteGroupBuilder reqMember = app.MapGroup("/api_req_member").WithTags("api_req_member");
reqMember.MapPost("/get_event_selected_reward", (ApiReqMemberGetEventSelectedRewardRequest _) => new ApiResponse<ApiReqMemberGetEventSelectedRewardResponse>());
reqMember.MapPost("/get_incentive", (ApiReqMemberGetIncentiveRequest _) => new ApiResponse<ApiReqMemberGetIncentiveResponse>());
reqMember.MapPost("/get_practice_enemyinfo", (ApiReqMemberGetPracticeEnemyinfoRequest _) => new ApiResponse<ApiReqMemberGetPracticeEnemyinfoResponse>());
reqMember.MapPost("/itemuse", (ApiReqMemberItemuseRequest _) => new ApiResponse<ApiReqMemberItemuseResponse>());
reqMember.MapPost("/itemuse_cond", (ApiReqMemberItemuseCondRequest _) => new ApiReqMemberItemuseCondResponse()); // todo
reqMember.MapPost("/payitemuse", (ApiReqMemberPayitemuseRequest _) => new ApiResponse<ApiReqMemberPayitemuseResponse>());
reqMember.MapPost("/set_flagship_position", (ApiReqMemberSetFlagshipPositionRequest _) => new ApiResponse<ApiReqMemberSetFlagshipPositionResponse>());
reqMember.MapPost("/set_friendly_request", (ApiReqMemberSetFriendlyRequestRequest _) => new ApiResponseNull<ApiReqMemberSetFriendlyRequestResponse>());
reqMember.MapPost("/set_oss_condition", (ApiReqMemberSetOssConditionRequest _) => new ApiResponse<ApiReqMemberSetOssConditionResponse>());
reqMember.MapPost("/updatecomment", (ApiReqMemberUpdatecommentRequest _) => new ApiResponseNull<ApiReqMemberUpdatecommentResponse>());
reqMember.MapPost("/updatedeckname", (ApiReqMemberUpdatedecknameRequest _) => new ApiResponseNull<ApiReqMemberUpdatedecknameResponse>());

RouteGroupBuilder reqMission = app.MapGroup("/api_req_mission").WithTags("api_req_mission");
reqMission.MapPost("/result", (ApiReqMissionResultRequest _) => new ApiResponse<ApiReqMissionResultResponse>());
reqMission.MapPost("/return_instruction", (ApiReqMissionReturnInstructionRequest _) => new ApiResponse<ApiReqMissionReturnInstructionResponse>());
reqMission.MapPost("/start", (ApiReqMissionStartRequest _) => new ApiResponse<ApiReqMissionStartResponse>());

RouteGroupBuilder reqNyukyo = app.MapGroup("/api_req_nyukyo").WithTags("api_req_nyukyo");
reqNyukyo.MapPost("/speedchange", (ApiReqNyukyoSpeedchangeRequest _) => new ApiResponse<ApiReqNyukyoSpeedchangeResponse>());
reqNyukyo.MapPost("/start", (ApiReqNyukyoStartRequest _) => new ApiResponseNull<APIReqNyukyoStartResponse>());

RouteGroupBuilder reqPractice = app.MapGroup("/api_req_practice").WithTags("api_req_practice");
reqPractice.MapPost("/battle", (ApiReqPracticeBattleRequest _) => new ApiResponse<ApiReqPracticeBattleResponse>());
reqPractice.MapPost("/battle_result", (ApiReqPracticeBattleResultRequest _) => new ApiResponse<ApiReqPracticeBattleResultResponse>());
reqPractice.MapPost("/change_matching_kind", (ApiReqPracticeChangeMatchingKindRequest _) => new ApiResponse<ApiReqPracticeChangeMatchingKindResponse>());
reqPractice.MapPost("/midnight_battle", (ApiReqPracticeMidnightBattleRequest _) => new ApiResponse<ApiReqPracticeMidnightBattleResponse>());

RouteGroupBuilder reqQuest = app.MapGroup("/api_req_quest").WithTags("api_req_quest");
reqQuest.MapPost("/clearitemget", (ApiReqQuestClearitemgetRequest _) => new ApiResponse<ApiReqQuestClearitemgetResponse>());
reqQuest.MapPost("/start", (ApiReqQuestStartRequest _) => new ApiResponse<ApiReqQuestStartResponse>());
reqQuest.MapPost("/stop", (ApiReqQuestStopRequest _) => new ApiResponse<ApiReqQuestStopResponse>());

RouteGroupBuilder reqRanking = app.MapGroup("/api_req_ranking").WithTags("api_req_ranking");
reqRanking.MapPost("/mxltvkpyuklh", (ApiReqRankingMxltvkpyuklhRequest _) => new ApiResponse<ApiReqRankingMxltvkpyuklhResponse>());

RouteGroupBuilder reqSortie = app.MapGroup("/api_req_sortie").WithTags("api_req_sortie");
reqSortie.MapPost("/airbattle", (ApiReqSortieAirbattleRequest _) => new ApiResponse<ApiReqSortieAirbattleResponse>());
reqSortie.MapPost("/battle", (ApiReqSortieBattleRequest _) => new ApiResponse<ApiReqSortieBattleResponse>());
reqSortie.MapPost("/battleresult", (ApiReqSortieBattleresultRequest _) => new ApiResponse<ApiReqSortieBattleresultResponse>());
reqSortie.MapPost("/go_back_port", (ApiReqSortieGobackPortRequest _) => new ApiResponseNull<ApiReqSortieGobackPortResponse>());
reqSortie.MapPost("/ld_airbattle", (ApiReqSortieLdAirbattleRequest _) => new ApiResponse<ApiReqSortieLdAirbattleResponse>());
reqSortie.MapPost("/ld_shooting", (ApiReqSortieLdShootingRequest _) => new ApiResponse<ApiReqSortieLdShootingResponse>());
reqSortie.MapPost("/night_to_day", (/* todo */) => new ApiResponse<ApiReqSortieNightToDayResponse>());

RouteGroupBuilder start2 = app.MapGroup("/api_start2").WithTags("api_start2");
start2.MapPost("/getData", (ApiStart2GetDataRequest _) => new ApiResponse<ApiStart2GetDataResponse>());

app.Run();
