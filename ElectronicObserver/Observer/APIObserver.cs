using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Controls;
using DynaJson;
using ElectronicObserver.Data;
using ElectronicObserver.Services.ApiFileService;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace ElectronicObserver.Observer;

public sealed class APIObserver
{
	private object LockObject { get; } = new();

	#region Singleton

	private static readonly APIObserver instance = new APIObserver();

	public static APIObserver Instance => instance;

	#endregion



	public APIDictionary APIList { get; private set; }

	#region API members

	/// <summary>
	/// Send ship to dock <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L838" />
	/// </summary>
	public kcsapi.api_req_nyukyo.start ApiReqNyukyo_Start { get; } = new();

	/// <summary>
	/// Use bucket on docked ship (this doesn't happen if you use a bucket while docking) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L846" />
	/// </summary>
	public kcsapi.api_req_nyukyo.speedchange ApiReqNyukyo_SpeedChange { get; } = new();

	/// <summary>
	/// Fleet reorganization <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1198" />
	/// </summary>
	public kcsapi.api_req_hensei.change ApiReqHensei_Change { get; } = new();

	/// <summary>
	/// Ship scrap <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L767" />
	/// </summary>
	public kcsapi.api_req_kousyou.destroyship ApiReqKousyou_DestroyShip { get; } = new();

	/// <summary>
	/// Change fleet name <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1239" />
	/// </summary>
	public kcsapi.api_req_member.updatedeckname ApiReqMember_UpdateDeckName { get; } = new();

	/// <summary>
	/// Ship remodel <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1499" />
	/// </summary>
	public kcsapi.api_req_kaisou.remodeling ApiReqKaisou_Remodeling { get; } = new();

	/// <summary>
	/// Sortie start <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1579" />
	/// </summary>
	public kcsapi.api_req_map.start ApiReqMap_Start { get; } = new();

	/// <summary>
	/// Make a combined fleet <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3375" />
	/// </summary>
	public kcsapi.api_req_hensei.combined ApiReqHensei_Combined { get; } = new();

	/// <summary>
	/// Ship hole-punch <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1460" />
	/// </summary>
	public kcsapi.api_req_kaisou.open_exslot ApiReqKaisou_OpenExSlot { get; } = new();

	/// <summary>
	/// Get to main screen (home port) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L603" />
	/// </summary>
	public kcsapi.api_port.port ApiPort_Port { get; } = new();

	/// <summary>
	/// Air base morale refresh call
	/// </summary>
	public kcsapi.api_port.airCorpsCondRecoveryWithTimer ApiPort_AirCorpsCondRecoveryWithTimer { get; } = new();

	/// <summary>
	/// 艦船情報 (?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2382" />
	/// </summary>
	public kcsapi.api_get_member.ship2 ApiGetMember_Ship2 { get; } = new();

	/// <summary>
	/// Go to dock screen (also happens after docking a ship, doesn't happen after bucketing a ship that's already in docks) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L826" />
	/// </summary>
	public kcsapi.api_get_member.ndock ApiGetMember_NDock { get; } = new();

	/// <summary>
	/// Get ship from construction <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L740" />
	/// </summary>
	public kcsapi.api_req_kousyou.getship ApiReqKousyou_GetShip { get; } = new();

	/// <summary>
	/// Resupply <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1163" />
	/// </summary>
	public kcsapi.api_req_hokyu.charge ApiReqHokyu_Charge { get; } = new();

	/// <summary>
	/// Happens when adding or removing equipment on a ship (doesn't happen when switching equipment between slots) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1505" />
	/// </summary>
	public kcsapi.api_get_member.ship3 ApiGetMember_Ship3 { get; } = new();

	/// <summary>
	/// Ship modernization <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1125" />
	/// </summary>
	public kcsapi.api_req_kaisou.powerup ApiReqKaisou_PowerUp { get; } = new();

	/// <summary>
	/// After sending out an expedition <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1188" />
	/// </summary>
	public kcsapi.api_get_member.deck ApiGetMember_Deck { get; } = new();

	/// <summary>
	/// After finishing a sortie <br />
	/// After finishing a quest that rewards ships or equipment <br />
	/// After remodeling a ship <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L685" />
	/// </summary>
	public kcsapi.api_get_member.slot_item ApiGetMember_SlotItem { get; } = new();

	/// <summary>
	/// Sortie advance <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1627" />
	/// </summary>
	public kcsapi.api_req_map.next ApiReqMap_Next { get; } = new();

	/// <summary>
	/// Sortie advance (right before api_req_map/next) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2392" />
	/// </summary>
	public kcsapi.api_get_member.ship_deck ApiGetMember_ShipDeck { get; } = new();

	/// <summary>
	/// Load fleet preset <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1225" />
	/// </summary>
	public kcsapi.api_req_hensei.preset_select ApiReqHensei_PresetSelect { get; } = new();

	/// <summary>
	/// Drag and drop equipment between slots <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1473" />
	/// </summary>
	public kcsapi.api_req_kaisou.slot_exchange_index ApiReqKaisou_SlotExchangeIndex { get; } = new();

	/// <summary>
	/// Login <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L553" />
	/// </summary>
	public kcsapi.api_get_member.require_info ApiGetMember_RequireInfo { get; } = new();

	/// <summary>
	/// Equipment transfer between ships <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1482" />
	/// </summary>
	public kcsapi.api_req_kaisou.slot_deprive ApiReqKaisou_SlotDeprive { get; } = new();

	/// <summary>
	/// Marriage <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3390" />
	/// </summary>
	public kcsapi.api_req_kaisou.marriage ApiReqKaisou_Marriage { get; } = new();

	/// <summary>
	/// Anchorage repair <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3520" />
	/// </summary>
	public kcsapi.api_req_map.anchorage_repair ApiReqMap_AnchorageRepair { get; } = new();

	/// <summary>
	/// Go to quest screen <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L892" />
	/// </summary>
	public kcsapi.api_get_member.questlist ApiGetMember_QuestList { get; } = new();

	/// <summary>
	/// Normal fleet battle finish <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2326" />
	/// </summary>
	public kcsapi.api_req_sortie.battleresult ApiReqSortie_BattleResult { get; } = new();

	/// <summary>
	/// Combined fleet battle finish <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3324" />
	/// </summary>
	public kcsapi.api_req_combined_battle.battleresult ApiReqCombinedBattle_BattleResult { get; } = new();

	/// <summary>
	/// Practice battle finish <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1427" />
	/// </summary>
	public kcsapi.api_req_practice.battle_result ApiReqPractice_BattleResult { get; } = new();

	/// <summary>
	/// Expedition finish <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1138" />
	/// </summary>
	public kcsapi.api_req_mission.result ApiReqMission_Result { get; } = new();

	/// <summary>
	/// Equipment development <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L749" />
	/// </summary>
	public kcsapi.api_req_kousyou.createitem ApiReqKousyou_CreateItem { get; } = new();

	/// <summary>
	/// Ship construction <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L720" />
	/// </summary>
	public kcsapi.api_req_kousyou.createship ApiReqKousyou_CreateShip { get; } = new();

	/// <summary>
	/// Equipment upgrade <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L811" />
	/// </summary>
	public kcsapi.api_req_kousyou.remodel_slot ApiReqKousyou_RemodelSlot { get; } = new();

	/// <summary>
	/// Login <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L9" />
	/// </summary>
	public kcsapi.api_start2.getData ApiStart2_GetData { get; } = new();

	/// <summary>
	/// Use torch on a ship that's in construction <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L733" />
	/// </summary>
	public kcsapi.api_req_kousyou.createship_speedchange ApiReqKousyou_CreateShipSpeedChange { get; } = new();

	/// <summary>
	/// Equipment scrap <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L775" />
	/// </summary>
	public kcsapi.api_req_kousyou.destroyitem2 ApiReqKousyou_DestroyItem2 { get; } = new();

	/// <summary>
	/// Change admiral comment <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1245" />
	/// </summary>
	public kcsapi.api_req_member.updatecomment ApiReqMember_UpdateComment { get; } = new();

	/// <summary>
	/// 艦隊司令部情報 (?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L568" />
	/// </summary>
	public kcsapi.api_get_member.basic ApiGetMember_Basic { get; } = new();

	/// <summary>
	/// After remodeling <br />
	/// After building a ship <br />
	/// After finishing a quest that rewards resources (fuel, ammo, steel, bauxite, bucket, torch, nail, screw) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L703" />
	/// </summary>
	public kcsapi.api_get_member.material ApiGetMember_Material { get; } = new();

	/// <summary>
	/// Add plane to AB <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2448" />
	/// </summary>
	public kcsapi.api_req_air_corps.set_plane ApiReqAirCorps_SetPlane { get; } = new();

	/// <summary>
	/// AB resupply <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2486" />
	/// </summary>
	public kcsapi.api_req_air_corps.supply ApiReqAirCorps_Supply { get; } = new();

	/// <summary>
	/// List of items <br />
	/// After finishing a sortie <br />
	/// After finishing an expedition <br />
	/// After finishing a quest that rewards an item <br />
	/// After marriage <br />
	/// After using Mamiya/Irako <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L698" />
	/// </summary>
	public kcsapi.api_get_member.useitem ApiGetMember_UseItem { get; } = new();

	/// <summary>
	/// Sortie screen
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2399" />
	/// </summary>
	public kcsapi.api_get_member.mapinfo ApiGetMember_MapInfo { get; } = new();

	/// <summary>
	/// Event sortie condition (sortie win rate needs to be above 75%) <br />
	/// Happens right before the sortie <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2426" />
	/// </summary>
	public kcsapi.api_get_member.sortie_conditions ApiGetMember_SortieConditions { get; } = new();

	/// <summary>
	/// Player details when clicking an opponent in practice <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1296" />
	/// </summary>
	public kcsapi.api_req_member.get_practice_enemyinfo ApiReqMember_GetPracticeEnemyInfo { get; } = new();

	/// <summary>
	/// Heavy air raid <br />
	/// todo: documentation
	/// </summary>
	public kcsapi.api_req_map.air_raid ApiReqMap_AirRaid { get; } = new();

	/// <summary>
	/// Normal fleet day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1752" />
	/// </summary>
	public kcsapi.api_req_sortie.battle ApiReqSortie_Battle { get; } = new();

	/// <summary>
	/// Normal fleet night battle after day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1967" />
	/// </summary>
	public kcsapi.api_req_battle_midnight.battle ApiReqBattleMidnight_Battle { get; } = new();

	/// <summary>
	/// Normal fleet night battle only <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2007" />
	/// </summary>
	public kcsapi.api_req_battle_midnight.sp_midnight ApiReqBattleMidnight_SpMidnight { get; } = new();

	/// <summary>
	/// Normal fleet air battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2033" />
	/// </summary>
	public kcsapi.api_req_sortie.airbattle ApiReqSortie_AirBattle { get; } = new();

	/// <summary>
	/// Normal fleet long distance air battle (like the one in 1-6?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2055" />
	/// </summary>
	public kcsapi.api_req_sortie.ld_airbattle ApiReqSortie_LdAirBattle { get; } = new();

	/// <summary>
	/// Normal fleet (?) night to day <br />
	/// todo: documentation
	/// </summary>
	public kcsapi.api_req_sortie.night_to_day ApiReqSortie_NightToDay { get; } = new();

	/// <summary>
	/// Normal fleet radar ambush <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2075" />
	/// </summary>
	public kcsapi.api_req_sortie.ld_shooting ApiReqSortie_LdShooting { get; } = new();

	/// <summary>
	/// Combined fleet (carrier or transport) day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2557" />
	/// </summary>
	public kcsapi.api_req_combined_battle.battle ApiReqCombinedBattle_Battle { get; } = new();

	/// <summary>
	/// Combined fleet vs normal fleet day to night battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2658" />
	/// </summary>
	public kcsapi.api_req_combined_battle.midnight_battle ApiReqCombinedBattle_MidnightBattle { get; } = new();

	/// <summary>
	/// Combined fleet night battle only <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2689" />
	/// </summary>
	public kcsapi.api_req_combined_battle.sp_midnight ApiReqCombinedBattle_SpMidnight { get; } = new();

	/// <summary>
	/// Combined fleet air battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2501" />
	/// </summary>
	public kcsapi.api_req_combined_battle.airbattle ApiReqCombinedBattle_AirBattle { get; } = new();

	/// <summary>
	/// Combined fleet (surface) day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2694" />
	/// </summary>
	public kcsapi.api_req_combined_battle.battle_water ApiReqCombinedBattle_BattleWater { get; } = new();

	/// <summary>
	/// Combined fleet long distance air battle (the ones from Midway event?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2802" />
	/// </summary>
	public kcsapi.api_req_combined_battle.ld_airbattle ApiReqCombinedBattle_LdAirBattle { get; } = new();

	/// <summary>
	/// Normal fleet vs enemy combined fleet day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2143" />
	/// </summary>
	public kcsapi.api_req_combined_battle.ec_battle ApiReqCombinedBattle_EcBattle { get; } = new();

	/// <summary>
	/// Normal/combined fleet vs enemy combined fleet day to night battle (?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2268" />
	/// </summary>
	public kcsapi.api_req_combined_battle.ec_midnight_battle ApiReqCombinedBattle_EcMidnightBattle { get; } = new();

	/// <summary>
	/// Enemy combined fleet night to day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3105" />
	/// </summary>
	public kcsapi.api_req_combined_battle.ec_night_to_day ApiReqCombinedBattle_EcNightToDay { get; } = new();

	/// <summary>
	/// Combined fleet (carrier) vs enemy combined fleet day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2859" />
	/// </summary>
	public kcsapi.api_req_combined_battle.each_battle ApiReqCombinedBattle_EachBattle { get; } = new();

	/// <summary>
	/// Combined fleet (surface) vs enemy combined fleet day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2964" />
	/// </summary>
	public kcsapi.api_req_combined_battle.each_battle_water ApiReqCombinedBattle_EachBattleWater { get; } = new();

	/// <summary>
	/// Combined fleet radar ambush <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3296" />
	/// </summary>
	public kcsapi.api_req_combined_battle.ld_shooting ApiReqCombinedBattle_LdShooting { get; } = new();

	/// <summary>
	/// Practice day battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1326" />
	/// </summary>
	public kcsapi.api_req_practice.battle ApiReqPractice_Battle { get; } = new();

	/// <summary>
	/// Practice night battle <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1402" />
	/// </summary>
	public kcsapi.api_req_practice.midnight_battle ApiReqPractice_MidnightBattle { get; } = new();

	/// <summary>
	/// Go to album <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L963" />
	/// </summary>
	public kcsapi.api_get_member.picture_book ApiGetMember_PictureBook { get; } = new();

	/// <summary>
	/// Go to expedition screen <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L853" />
	/// </summary>
	public kcsapi.api_get_member.mission ApiGetMember_Mission { get; } = new();

	/// <summary>
	/// Send out expedition <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1178" />
	/// </summary>
	public kcsapi.api_req_mission.start ApiReqMission_Start { get; } = new();

	/// <summary>
	/// Switching AB planes (?) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2433" />
	/// </summary>
	public kcsapi.api_get_member.base_air_corps ApiGetMember_BaseAirCorps { get; } = new();

	/// <summary>
	/// Move AB planes between different bases <br />
	/// todo: documentation
	/// </summary>
	public kcsapi.api_req_air_corps.change_deployment_base ApiReqAirCorps_ChangeDeploymentBase { get; } = new();

	/// <summary>
	/// Change AB name <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2461" />
	/// </summary>
	public kcsapi.api_req_air_corps.change_name ApiReqAirCorps_ChangeName { get; } = new();

	/// <summary>
	/// Change AB action (sortie, air def, rest etc) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2469" />
	/// </summary>
	public kcsapi.api_req_air_corps.set_action ApiReqAirCorps_SetAction { get; } = new();

	/// <summary>
	/// Open new AB <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2496" />
	/// </summary>
	public kcsapi.api_req_air_corps.expand_base ApiReqAirCorps_ExpandBase { get; } = new();

	/// <summary>
	/// Ship construction <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L708" />
	/// </summary>
	public kcsapi.api_get_member.kdock ApiGetMember_KDock { get; } = new();

	/// <summary>
	/// Quest clear <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L934" />
	/// </summary>
	public kcsapi.api_req_quest.clearitemget ApiReqQuest_ClearItemGet { get; } = new();

	/// <summary>
	/// Admiral profile <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L859" />
	/// </summary>
	public kcsapi.api_get_member.record ApiGetMember_Record { get; } = new();

	/// <summary>
	/// Item page <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3397" />
	/// </summary>
	public kcsapi.api_get_member.payitem ApiGetMember_PayItem { get; } = new();

	/// <summary>
	/// Akashi arsenal page <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L782" />
	/// </summary>
	public kcsapi.api_req_kousyou.remodel_slotlist ApiReqKousyou_RemodelSlotList { get; } = new();

	/// <summary>
	/// Akashi arsenal upgrade cost detail after selecting the equipment to upgrade <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L798" />
	/// </summary>
	public kcsapi.api_req_kousyou.remodel_slotlist_detail ApiReqKousyou_RemodelSlotListDetail { get; } = new();

	/// <summary>
	/// FCF <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3368" />
	/// </summary>
	public kcsapi.api_req_combined_battle.goback_port ApiReqCombinedBattle_GoBackPort { get; } = new();

	/// <summary>
	/// Ranking list <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1265" />
	/// </summary>
	public kcsapi.api_req_ranking.mxltvkpyuklh ApiReqRanking_Mxltvkpyuklh { get; } = new();

	/// <summary>
	/// FCF single fleet <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2378" />
	/// </summary>
	public kcsapi.api_req_sortie.goback_port ApiReqSortie_GoBackPort { get; } = new();

	/// <summary>
	/// Item exchange (?) (eg. medals -> blueprint) <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3414" />
	/// </summary>
	public kcsapi.api_req_member.itemuse ApiReqMember_ItemUse { get; } = new();

	/// <summary>
	/// Send AB to a node on map <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L2478" />
	/// </summary>
	public kcsapi.api_req_map.start_air_base ApiReqMap_StartAirBase { get; } = new();

	/// <summary>
	/// Preset data - open fleet organization screen <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1207" />
	/// </summary>
	public kcsapi.api_get_member.preset_deck ApiGetMember_PresetDeck { get; } = new();

	/// <summary>
	/// Select event difficulty <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L3457" />
	/// </summary>
	public kcsapi.api_req_map.select_eventmap_rank ApiReqMap_SelectEventMapRank { get; } = new();

	/// <summary>
	/// Deactivate quest <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L927" />
	/// </summary>
	public kcsapi.api_req_quest.stop ApiReqQuest_Stop { get; } = new();

	/// <summary>
	/// Save fleet preset <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1217" />
	/// </summary>
	public kcsapi.api_req_hensei.preset_register ApiReqHensei_PresetRegister { get; } = new();

	/// <summary>
	/// Delete fleet preset <br />
	/// <seealso href="https://github.com/andanteyk/ElectronicObserver/blob/develop/ElectronicObserver/Other/Information/apilist.txt#L1232" />
	/// </summary>
	public kcsapi.api_req_hensei.preset_delete ApiReqHensei_PresetDelete { get; } = new();

	#endregion

	public string? ServerAddress { get; private set; }
	public int ProxyPort { get; private set; }

	public delegate void ProxyStartedEventHandler();
	public event ProxyStartedEventHandler ProxyStarted = delegate { };

	private Control UIControl { get; set; }


	public event APIReceivedEventHandler RequestReceived = delegate { };
	public event APIReceivedEventHandler ResponseReceived = delegate { };

	private ProxyServer Proxy { get; }
	private ExplicitProxyEndPoint Endpoint { get; set; }

	private Lazy<ApiFileService> ApiFileService { get; } = new(() => new(KCDatabase.Instance));

	private Channel<Action> ApiProcessingChannel { get; } = Channel.CreateUnbounded<Action>(new UnboundedChannelOptions
	{
		SingleReader = true,
		SingleWriter = true,
		AllowSynchronousContinuations = true,
	});

	private APIObserver()
	{
		APIList = new APIDictionary
		{
			ApiStart2_GetData,
			ApiGetMember_Basic,
			ApiGetMember_SlotItem,
			ApiGetMember_UseItem,
			ApiGetMember_KDock,
			ApiPort_Port,
			ApiPort_AirCorpsCondRecoveryWithTimer,
			ApiGetMember_Ship2,
			ApiGetMember_QuestList,
			ApiGetMember_NDock,
			ApiReqKousyou_GetShip,
			ApiReqHokyu_Charge,
			ApiReqKousyou_DestroyShip,
			ApiReqKousyou_DestroyItem2,
			ApiReqMember_GetPracticeEnemyInfo,
			ApiGetMember_PictureBook,
			ApiReqMission_Start,
			ApiGetMember_Ship3,
			ApiReqKaisou_PowerUp,
			ApiReqMap_Start,
			ApiReqMap_Next,
			ApiReqMap_AirRaid,
			ApiReqKousyou_CreateItem,
			ApiReqSortie_Battle,
			ApiReqSortie_BattleResult,
			ApiReqBattleMidnight_Battle,
			ApiReqBattleMidnight_SpMidnight,
			ApiReqCombinedBattle_Battle,
			ApiReqCombinedBattle_MidnightBattle,
			ApiReqCombinedBattle_SpMidnight,
			ApiReqCombinedBattle_AirBattle,
			ApiReqCombinedBattle_BattleResult,
			ApiReqPractice_Battle,
			ApiReqPractice_MidnightBattle,
			ApiReqPractice_BattleResult,
			ApiGetMember_Deck,
			ApiGetMember_MapInfo,
			ApiGetMember_Mission,
			ApiReqCombinedBattle_BattleWater,
			ApiReqCombinedBattle_GoBackPort,
			ApiReqKousyou_RemodelSlot,
			ApiGetMember_Material,
			ApiReqMission_Result,
			ApiReqRanking_Mxltvkpyuklh,
			ApiReqSortie_AirBattle,
			ApiGetMember_ShipDeck,
			ApiReqKaisou_Marriage,
			ApiReqHensei_PresetSelect,
			ApiReqKaisou_SlotExchangeIndex,
			ApiGetMember_Record,
			ApiGetMember_PayItem,
			ApiReqKousyou_RemodelSlotList,
			ApiReqKousyou_RemodelSlotListDetail,
			ApiReqSortie_LdAirBattle,
			ApiReqCombinedBattle_LdAirBattle,
			ApiGetMember_RequireInfo,
			ApiGetMember_BaseAirCorps,
			ApiReqAirCorps_ChangeDeploymentBase,
			ApiReqAirCorps_SetPlane,
			ApiReqAirCorps_SetAction,
			ApiReqAirCorps_Supply,
			ApiReqKaisou_SlotDeprive,
			ApiReqAirCorps_ExpandBase,
			ApiReqCombinedBattle_EcBattle,
			ApiReqCombinedBattle_EcMidnightBattle,
			ApiReqCombinedBattle_EachBattle,
			ApiReqCombinedBattle_EachBattleWater,
			ApiGetMember_SortieConditions,
			ApiReqSortie_NightToDay,
			ApiReqCombinedBattle_EcNightToDay,
			ApiReqSortie_GoBackPort,
			ApiReqMember_ItemUse,
			ApiReqSortie_LdShooting,
			ApiReqCombinedBattle_LdShooting,
			ApiReqMap_AnchorageRepair,
			ApiReqMap_StartAirBase,
			ApiGetMember_PresetDeck,

			ApiReqQuest_ClearItemGet,
			ApiReqNyukyo_Start,
			ApiReqNyukyo_SpeedChange,
			ApiReqKousyou_CreateShip,
			ApiReqKousyou_CreateShipSpeedChange,
			ApiReqHensei_Change,
			ApiReqMember_UpdateDeckName,
			ApiReqKaisou_Remodeling,
			ApiReqKaisou_OpenExSlot,
			ApiReqMap_SelectEventMapRank,
			ApiReqHensei_Combined,
			ApiReqMember_UpdateComment,
			ApiReqAirCorps_ChangeName,
			ApiReqQuest_Stop,
			ApiReqHensei_PresetRegister,
			ApiReqHensei_PresetDelete,
		};

		Proxy = new ProxyServer
		{
			ExceptionFunc = async exception =>
			{
				// todo write to output
			},
			TcpTimeWaitSeconds = 10,
			ConnectionTimeOutSeconds = 15,
			ReuseSocket = false,
			EnableConnectionPool = false,
			ForwardToUpstreamGateway = true,
		};

		Proxy.BeforeRequest += ProxyOnBeforeRequest;
		Proxy.BeforeResponse += ProxyOnBeforeResponse;

		Task.Run(ProcessApiDataAsync);
	}

	private async Task ProcessApiDataAsync()
	{
		// basically while (true)
		while (await ApiProcessingChannel.Reader.WaitToReadAsync())
		{
			Action apiAction = await ApiProcessingChannel.Reader.ReadAsync();
			await UIControl.Dispatcher.BeginInvoke(apiAction);
		}
	}

	/// <summary>
	/// 通信の受信を開始します。
	/// </summary>
	/// <param name="portID">受信に使用するポート番号。</param>
	/// <param name="uiControl">GUI スレッドで実行するためのオブジェクト。中身は何でもいい</param>
	/// <returns>実際に使用されるポート番号。</returns>
	public int Start(int portID, Control uiControl)
	{
		Configuration.ConfigurationData.ConfigConnection c = Configuration.Config.Connection;

		UIControl = uiControl;

		if (Proxy.ProxyRunning)
		{
			Proxy.Stop();
		}

		try
		{
			Endpoint = new ExplicitProxyEndPoint(IPAddress.Any, portID, true);
			Proxy.AddEndPoint(Endpoint);

			ExternalProxy? upstreamProxy = c switch
			{
				{ UseUpstreamProxy: true } => new(c.UpstreamProxyAddress, c.UpstreamProxyPort),
				_ => null,
			};

			Proxy.UpStreamHttpProxy = upstreamProxy;
			Proxy.UpStreamHttpsProxy = upstreamProxy;

			Proxy.Start();
			ProxyPort = portID;

			ProxyStarted();

			Logger.Add(1, string.Format(LoggerRes.APIObserverStarted, portID));

		}
		catch (Exception ex)
		{
			Logger.Add(3, ObserverRes.APIObserverFailed, ex);
			ProxyPort = 0;
		}

		return ProxyPort;
	}

	/// <summary>
	/// 通信の受信を停止します。
	/// </summary>
	public void Stop()
	{
		Proxy.Stop();

		Logger.Add(1, LoggerRes.APIObserverStopped);
	}

	private async Task ProxyOnBeforeRequest(object sender, SessionEventArgs e)
	{
		e.HttpClient.Request.KeepBody = true;

		if (e.HttpClient.Request.RequestUri.AbsoluteUri.Contains("/kcsapi/"))
		{
			// need to read the request body here so it's available in ProxyOnBeforeResponse
			await e.GetRequestBodyAsString();
		}
	}

	private async Task ProxyOnBeforeResponse(object sender, SessionEventArgs e)
	{
		if (e.HttpClient.Response.StatusCode is not (200 or 206)) return;
		Configuration.ConfigurationData.ConfigConnection c = Configuration.Config.Connection;

		string baseurl = e.HttpClient.Request.RequestUri.AbsoluteUri;

		if (baseurl.Contains("/kcsapi/"))
		{
			string apiName = baseurl.Split("/kcsapi/").Last();
			string requestBody = await e.GetRequestBodyAsString();
			string responseBody = await e.GetResponseBodyAsString();

			await ApiFileService.Value.Add(apiName, requestBody, responseBody);
		}

		// request
		if (baseurl.Contains("/kcsapi/"))
		{

			string url = baseurl;
			string body = await e.GetRequestBodyAsString();

			//保存
			if (c.SaveReceivedData && c.SaveRequest)
			{
				Task.Run((Action)(() => { SaveRequest(url, body); }));
			}

			await ApiProcessingChannel.Writer.WriteAsync(() => LoadRequest(url, body));
		}

		if (KCDatabase.Instance.ServerManager.CurrentServer is null && baseurl.Contains("/gadget_html5/js/kcs_const.js"))
		{
			string body = await e.GetResponseBodyAsString();

			_ = Task.Run(() =>
			{
				KCDatabase.Instance.ServerManager.LoadServerList(body);
			});

			return;
		}

		//response
		//保存
		if (c.SaveReceivedData)
		{

			try
			{

				if (!Directory.Exists(c.SaveDataPath))
					Directory.CreateDirectory(c.SaveDataPath);


				if (c.SaveResponse && baseurl.Contains("/kcsapi/"))
				{

					// 非同期で書き出し処理するので取っておく
					// stringはイミュータブルなのでOK
					string url = baseurl;
					string body = await e.GetResponseBodyAsString();

					Task.Run((Action)(() =>
					{
						SaveResponse(url, body);
					}));

				}
				else if (baseurl.Contains("/kcs") && c.SaveOtherFile)
				{

					string saveDataPath = c.SaveDataPath; // スレッド間の競合を避けるため取っておく

					string tpath = string.Format("{0}\\{1}", saveDataPath, baseurl.Substring(baseurl.IndexOf("/kcs") + 1).Replace("/", "\\"));
					//Logger.Add(1, $"{baseurl} $ {tpath}");
					{
						int index = tpath.IndexOf("?");
						if (index != -1)
						{
							if (Configuration.Config.Connection.ApplyVersion)
							{
								string over = tpath.Substring(index + 1);
								int vindex = over.LastIndexOf("VERSION=", StringComparison.CurrentCultureIgnoreCase);
								if (vindex != -1)
								{
									string version = over.Substring(vindex + 8).Replace('.', '_');
									tpath = tpath.Insert(tpath.LastIndexOf('.', index), "_v" + version);
									index += version.Length + 2;
								}

							}

							tpath = tpath.Remove(index);
						}
					}



					// 非同期で書き出し処理するので取っておく
					byte[] responseCopy = new byte[(await e.GetResponseBody()).Length];
					Array.Copy(await e.GetResponseBody(), responseCopy, (await e.GetResponseBody()).Length);

					Task.Run((() =>
					{
						try
						{
							lock (LockObject)
							{
								// 同時に書き込みが走るとアレなのでロックしておく

								Directory.CreateDirectory(Path.GetDirectoryName(tpath));

								//System.Diagnostics.Debug.WriteLine( oSession.fullUrl + " => " + tpath );
								File.WriteAllBytesAsync(tpath, responseCopy);
							}

							Logger.Add(1, string.Format(LoggerRes.SavedAPI, tpath.Remove(0, saveDataPath.Length + 1)));

						}
						catch (IOException ex)
						{   //ファイルがロックされている; 頻繁に出るのでエラーレポートを残さない

							Logger.Add(3, LoggerRes.FailedSaveAPI + ex.Message);
						}
					}));

				}

			}
			catch (Exception ex)
			{

				ErrorReporter.SendErrorReport(ex, LoggerRes.FailedSaveAPI);
			}

		}

		if (baseurl.Contains("/kcsapi/") && e.HttpClient.Response.ContentType == "text/plain")
		{
			// 非同期でGUIスレッドに渡すので取っておく
			// stringはイミュータブルなのでOK
			string url = baseurl;
			string body = await e.GetResponseBodyAsString();

			await ApiProcessingChannel.Writer.WriteAsync(() => LoadResponse(url, body));
		}


		if (ServerAddress == null && baseurl.Contains("/kcsapi/"))
		{
			ServerAddress = e.HttpClient.Request.Host;

			if (!string.IsNullOrEmpty(ServerAddress))
			{
				KCDatabase.Instance.ServerManager.LoadCurrentServer(ServerAddress);
			}
		}
	}

	public void LoadRequest(string path, string data)
	{

		string shortpath = path.Substring(path.LastIndexOf("/kcsapi/") + 8);

		try
		{

			Logger.Add(1, LoggerRes.RecievedRequest + shortpath);

			SystemEvents.UpdateTimerEnabled = false;


			var parsedData = new Dictionary<string, string>();


			foreach (string unit in data.Split("&".ToCharArray()))
			{
				string[] pair = unit.Split("=".ToCharArray());
				parsedData.Add(HttpUtility.UrlDecode(pair[0]), HttpUtility.UrlDecode(pair[1]));
			}

			RequestReceived(shortpath, parsedData);
			APIList.OnRequestReceived(shortpath, parsedData);

		}
		catch (Exception ex)
		{

			ErrorReporter.SendErrorReport(ex, LoggerRes.RequestError, shortpath, data);

		}
		finally
		{

			SystemEvents.UpdateTimerEnabled = true;

		}

	}


	public void LoadResponse(string path, string data)
	{

		string shortpath = path.Substring(path.LastIndexOf("/kcsapi/") + 8);

		try
		{

			Logger.Add(1, ObserverRes.ResponseRecieved + shortpath);

			SystemEvents.UpdateTimerEnabled = false;


			var json = JsonObject.Parse(data.Substring(7));        //remove "svdata="

			int result = (int)json.api_result;
			if (result != 1)
			{
				if (result == 201)
				{
					Logger.Add(3,
						ObserverRes.Error201);
				}
				else
				{
					throw new InvalidOperationException(string.Format(ObserverRes.ErrorFromServer, result));
				}
			}


			if (shortpath == "api_get_member/ship2")
			{
				ResponseReceived(shortpath, json);
				APIList.OnResponseReceived(shortpath, json);
			}
			else if (json.IsDefined("api_data"))
			{
				ResponseReceived(shortpath, json.api_data);
				APIList.OnResponseReceived(shortpath, json.api_data);
			}
			else
			{
				ResponseReceived(shortpath, null);
				APIList.OnResponseReceived(shortpath, null);
			}

			Task.Run(() => ApiFileService.Value.ProcessedApi(shortpath));
		}
		catch (Exception ex)
		{

			ErrorReporter.SendErrorReport(ex,
				ObserverRes.ErrorResponse,
				shortpath, data);

		}
		finally
		{

			SystemEvents.UpdateTimerEnabled = true;

		}

	}


	private void SaveRequest(string url, string body)
	{

		try
		{

			string tpath = string.Format("{0}\\{1}Q@{2}.json", Configuration.Config.Connection.SaveDataPath, DateTimeHelper.GetTimeStamp(), url.Substring(url.LastIndexOf("/kcsapi/") + 8).Replace("/", "@"));

			using (var sw = new StreamWriter(tpath, false, Encoding.UTF8))
			{
				sw.Write(body);
			}


		}
		catch (Exception ex)
		{

			ErrorReporter.SendErrorReport(ex, LoggerRes.FailedSaveAPI);

		}
	}


	private void SaveResponse(string url, string body)
	{

		try
		{

			string tpath = string.Format("{0}\\{1}S@{2}.json", Configuration.Config.Connection.SaveDataPath, DateTimeHelper.GetTimeStamp(), url.Substring(url.LastIndexOf("/kcsapi/") + 8).Replace("/", "@"));

			using (var sw = new StreamWriter(tpath, false, Encoding.UTF8))
			{
				sw.Write(body);
			}

		}
		catch (Exception ex)
		{

			ErrorReporter.SendErrorReport(ex, LoggerRes.FailedSaveAPI);

		}



	}

}
