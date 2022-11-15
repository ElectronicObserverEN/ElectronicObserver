using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;

public class ApiReqSortieBattleresultResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_dests")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiDests { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_destsf")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiDestsf { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_enemy_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiEnemyInfo ApiEnemyInfo { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_escape")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public object? ApiEscape { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_escape_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiEscapeFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_first_clear")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFirstClear { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_base_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiGetBaseExp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_eventflag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiGetEventflag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_eventitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<ApiGetEventitem>? ApiGetEventitem { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[System.Text.Json.Serialization.JsonPropertyName("api_get_exmap_rate")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public object ApiGetExmapRate { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[System.Text.Json.Serialization.JsonPropertyName("api_get_exmap_useitem_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public object ApiGetExmapUseitemId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiGetExp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_exp_lvup")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_get_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiGetFlag { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_get_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiGetShip? ApiGetShip { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_ship_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiGetShipExp { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_get_useitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiGetUseitem? ApiGetUseitem { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_landing_hp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiLandingHp? ApiLandingHp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_m1")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiM1 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_m_suffix")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiMSuffix { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_mapcell_incentive")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMapcellIncentive { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_member_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMemberExp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_member_lv")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMemberLv { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_mvp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMvp { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[System.Text.Json.Serialization.JsonPropertyName("api_next_map_ids")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<object>? ApiNextMapIds { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_quest_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiQuestLevel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_quest_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiQuestName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipId { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_win_rank")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiWinRank { get; set; } = default!;
}
