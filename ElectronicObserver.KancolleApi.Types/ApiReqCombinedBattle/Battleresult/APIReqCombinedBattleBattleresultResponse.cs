using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;

public class ApiReqCombinedBattleBattleresultResponse : ISortieBattleResultApi
{
	[JsonPropertyName("api_dests")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDests { get; set; } = default!;

	[JsonPropertyName("api_destsf")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDestsf { get; set; } = default!;

	[JsonPropertyName("api_enemy_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiEnemyInfo ApiEnemyInfo { get; set; } = new();

	[JsonPropertyName("api_escape")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiEscape ApiEscape { get; set; } = default!;

	[JsonPropertyName("api_escape_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiEscapeFlag { get; set; } = default!;

	[JsonPropertyName("api_first_clear")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFirstClear { get; set; } = default!;

	[JsonPropertyName("api_get_base_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiGetBaseExp { get; set; } = default!;

	[JsonPropertyName("api_get_eventflag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiGetEventflag { get; set; } = default!;

	[JsonPropertyName("api_get_eventitem")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<ApiGetEventitem>? ApiGetEventitem { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_get_exmap_rate")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public object ApiGetExmapRate { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_get_exmap_useitem_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public object ApiGetExmapUseitemId { get; set; } = default!;

	[JsonPropertyName("api_get_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiGetExp { get; set; } = default!;

	[JsonPropertyName("api_get_exp_lvup")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[JsonPropertyName("api_get_exp_lvup_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<List<int>>? ApiGetExpLvupCombined { get; set; }

	[JsonPropertyName("api_get_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiGetFlag { get; set; } = new();

	[JsonPropertyName("api_get_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiGetShip? ApiGetShip { get; set; } = default!;

	[JsonPropertyName("api_get_ship_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiGetShipExp { get; set; } = new();

	[JsonPropertyName("api_get_ship_exp_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiGetShipExpCombined { get; set; }

	[JsonPropertyName("api_m1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiM1 { get; set; } = default!;

	[JsonPropertyName("api_m_suffix")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiMSuffix { get; set; } = default!;

	[JsonPropertyName("api_member_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMemberExp { get; set; } = default!;

	[JsonPropertyName("api_member_lv")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMemberLv { get; set; } = default!;

	[JsonPropertyName("api_mvp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMvp { get; set; } = default!;

	[JsonPropertyName("api_mvp_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiMvpCombined { get; set; }

	[JsonPropertyName("api_next_map_ids")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<string>? ApiNextMapIds { get; set; } = default!;

	[JsonPropertyName("api_ope_suffix")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiOpeSuffix { get; set; } = default!;

	[JsonPropertyName("api_quest_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiQuestLevel { get; set; } = default!;

	[JsonPropertyName("api_quest_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiQuestName { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_win_rank")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiWinRank { get; set; } = default!;
}
