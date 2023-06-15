using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;

public class ApiReqCombinedBattleBattleresultResponse : ISortieBattleResultApi
{
	[JsonPropertyName("api_dests")]
	public int ApiDests { get; set; } = default!;

	[JsonPropertyName("api_destsf")]
	public int ApiDestsf { get; set; } = default!;

	[JsonPropertyName("api_enemy_info")]
	[Required]
	public ApiEnemyInfo ApiEnemyInfo { get; set; } = new();

	[JsonPropertyName("api_escape")]
	public ApiEscape ApiEscape { get; set; } = default!;

	[JsonPropertyName("api_escape_flag")]
	public int ApiEscapeFlag { get; set; } = default!;

	[JsonPropertyName("api_first_clear")]
	public int ApiFirstClear { get; set; } = default!;

	[JsonPropertyName("api_get_base_exp")]
	public int ApiGetBaseExp { get; set; } = default!;

	[JsonPropertyName("api_get_eventflag")]
	public int? ApiGetEventflag { get; set; } = default!;

	[JsonPropertyName("api_get_eventitem")]
	public List<ApiGetEventitem>? ApiGetEventitem { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_get_exmap_rate")]
	public object ApiGetExmapRate { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_get_exmap_useitem_id")]
	public object ApiGetExmapUseitemId { get; set; } = default!;

	[JsonPropertyName("api_get_exp")]
	public int ApiGetExp { get; set; } = default!;

	[JsonPropertyName("api_get_exp_lvup")]
	[Required]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[JsonPropertyName("api_get_exp_lvup_combined")]
	public List<List<int>>? ApiGetExpLvupCombined { get; set; }

	[JsonPropertyName("api_get_flag")]
	[Required]
	public List<int> ApiGetFlag { get; set; } = new();

	[JsonPropertyName("api_get_ship")]
	public ApiGetShip? ApiGetShip { get; set; } = default!;

	[JsonPropertyName("api_get_ship_exp")]
	[Required]
	public List<int> ApiGetShipExp { get; set; } = new();

	[JsonPropertyName("api_get_ship_exp_combined")]
	public List<int>? ApiGetShipExpCombined { get; set; }

	[JsonPropertyName("api_m1")]
	public int? ApiM1 { get; set; } = default!;

	[JsonPropertyName("api_m_suffix")]
	public string? ApiMSuffix { get; set; } = default!;

	[JsonPropertyName("api_member_exp")]
	public int ApiMemberExp { get; set; } = default!;

	[JsonPropertyName("api_member_lv")]
	public int ApiMemberLv { get; set; } = default!;

	[JsonPropertyName("api_mvp")]
	public int ApiMvp { get; set; } = default!;

	[JsonPropertyName("api_mvp_combined")]
	public int? ApiMvpCombined { get; set; }

	[JsonPropertyName("api_next_map_ids")]
	public List<object>? ApiNextMapIds { get; set; } = default!;

	[JsonPropertyName("api_ope_suffix")]
	public string? ApiOpeSuffix { get; set; } = default!;

	[JsonPropertyName("api_quest_level")]
	public int ApiQuestLevel { get; set; } = default!;

	[JsonPropertyName("api_quest_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiQuestName { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[Required]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_win_rank")]
	[Required(AllowEmptyStrings = true)]
	public string ApiWinRank { get; set; } = default!;

	[JsonPropertyName("api_get_useitem")]
	public ApiGetUseitem? ApiGetUseitem { get; set; } = default!;
	
	public ApiLandingHp? ApiLandingHp { get; set; }

	public int ApiMapcellIncentive { get; set; }
}
