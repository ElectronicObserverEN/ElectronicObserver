using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;

public class ApiReqSortieBattleresultResponse : ISortieBattleResultApi
{
	[JsonPropertyName("api_dests")]
	public int ApiDests { get; set; }

	[JsonPropertyName("api_destsf")]
	public int ApiDestsf { get; set; }

	[JsonPropertyName("api_enemy_info")]
	public ApiEnemyInfo ApiEnemyInfo { get; set; } = new();

	[JsonPropertyName("api_escape")]
	public ApiEscape? ApiEscape { get; set; }

	[JsonPropertyName("api_escape_flag")]
	public int ApiEscapeFlag { get; set; }

	[JsonPropertyName("api_first_clear")]
	public int ApiFirstClear { get; set; }

	[JsonPropertyName("api_get_base_exp")]
	public int ApiGetBaseExp { get; set; }

	[JsonPropertyName("api_get_eventflag")]
	public int? ApiGetEventflag { get; set; }

	[JsonPropertyName("api_get_eventitem")]
	public List<ApiGetEventitem>? ApiGetEventitem { get; set; }

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_get_exmap_rate")]
	public object ApiGetExmapRate { get; set; } = 0;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_get_exmap_useitem_id")]
	public object ApiGetExmapUseitemId { get; set; } = 0;

	[JsonPropertyName("api_get_exp")]
	public int ApiGetExp { get; set; }

	[JsonPropertyName("api_get_exp_lvup")]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[JsonPropertyName("api_get_flag")]
	public List<int> ApiGetFlag { get; set; } = new();

	[JsonPropertyName("api_get_ship")]
	public ApiGetShip? ApiGetShip { get; set; }

	[JsonPropertyName("api_get_ship_exp")]
	public List<int> ApiGetShipExp { get; set; } = new();

	[JsonPropertyName("api_get_useitem")]
	public ApiGetUseitem? ApiGetUseitem { get; set; }

	[JsonPropertyName("api_landing_hp")]
	public ApiLandingHp? ApiLandingHp { get; set; }

	[JsonPropertyName("api_m1")]
	public int? ApiM1 { get; set; }

	[JsonPropertyName("api_m_suffix")]
	public string? ApiMSuffix { get; set; }

	[JsonPropertyName("api_mapcell_incentive")]
	public int ApiMapcellIncentive { get; set; }

	[JsonPropertyName("api_member_exp")]
	public int ApiMemberExp { get; set; }

	[JsonPropertyName("api_member_lv")]
	public int ApiMemberLv { get; set; }

	[JsonPropertyName("api_mvp")]
	public int ApiMvp { get; set; }

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_next_map_ids")]
	public List<object>? ApiNextMapIds { get; set; }

	[JsonPropertyName("api_quest_level")]
	public int ApiQuestLevel { get; set; }

	[JsonPropertyName("api_quest_name")]
	public string ApiQuestName { get; set; } = "";

	[JsonPropertyName("api_ship_id")]
	public List<int> ApiShipId { get; set; } = new();

	// todo: this should be an enum
	[JsonPropertyName("api_win_rank")]
	public string ApiWinRank { get; set; } = "";
}
