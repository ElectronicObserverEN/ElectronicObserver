using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;

public class ApiReqCombinedBattleBattleresultResponse : ISortieBattleResultApi
{
	/// <inheritdoc />
	[JsonPropertyName("api_dests")]
	public int ApiDests { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_destsf")]
	public int ApiDestsf { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_enemy_info")]
	public ApiEnemyInfo ApiEnemyInfo { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_escape")]
	public ApiEscape? ApiEscape { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_escape_flag")]
	public int ApiEscapeFlag { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_first_clear")]
	public int ApiFirstClear { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_get_base_exp")]
	public int ApiGetBaseExp { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_get_eventflag")]
	public int? ApiGetEventflag { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_get_eventitem")]
	public List<ApiGetEventitem>? ApiGetEventitem { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_get_exmap_rate")]
	public object ApiGetExmapRate { get; set; } = 0;

	/// <inheritdoc />
	[JsonPropertyName("api_get_exmap_useitem_id")]
	public object ApiGetExmapUseitemId { get; set; } = 0;

	/// <inheritdoc />
	[JsonPropertyName("api_get_exp")]
	public int ApiGetExp { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_get_exp_lvup")]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[JsonPropertyName("api_get_exp_lvup_combined")]
	public List<List<int>>? ApiGetExpLvupCombined { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_get_flag")]
	public List<int> ApiGetFlag { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_get_ship")]
	public ApiGetShip? ApiGetShip { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_get_ship_exp")]
	public List<int> ApiGetShipExp { get; set; } = new();

	[JsonPropertyName("api_get_ship_exp_combined")]
	public List<int>? ApiGetShipExpCombined { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_m1")]
	public int? ApiM1 { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_m_suffix")]
	public string? ApiMSuffix { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_member_exp")]
	public int ApiMemberExp { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_member_lv")]
	public int ApiMemberLv { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_mvp")]
	public int ApiMvp { get; set; }

	[JsonPropertyName("api_mvp_combined")]
	public int? ApiMvpCombined { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_next_map_ids")]
	public List<object>? ApiNextMapIds { get; set; }

	[JsonPropertyName("api_ope_suffix")]
	public string? ApiOpeSuffix { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_quest_level")]
	public int ApiQuestLevel { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_quest_name")]
	public string ApiQuestName { get; set; } = "";

	/// <inheritdoc />
	[JsonPropertyName("api_ship_id")]
	public List<int> ApiShipId { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_win_rank")]
	public string ApiWinRank { get; set; } = "";

	/// <inheritdoc />
	[JsonPropertyName("api_get_useitem")]
	public ApiGetUseitem? ApiGetUseitem { get; set; }

	/// <inheritdoc />
	public ApiLandingHp? ApiLandingHp { get; set; }

	/// <inheritdoc />
	public int ApiMapcellIncentive { get; set; }
}
