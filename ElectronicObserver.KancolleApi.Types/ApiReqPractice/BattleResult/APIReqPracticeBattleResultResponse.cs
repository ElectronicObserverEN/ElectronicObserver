
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;

public class ApiReqPracticeBattleResultResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_enemy_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiEnemyInfo ApiEnemyInfo { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_get_base_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiGetBaseExp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiGetExp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_exp_lvup")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_get_ship_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiGetShipExp { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_member_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMemberExp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_member_lv")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMemberLv { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_mvp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMvp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipId { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_win_rank")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiWinRank { get; set; } = default!;
}
