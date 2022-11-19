
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;

public class ApiReqPracticeBattleResultResponse
{
	[JsonPropertyName("api_enemy_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiEnemyInfo ApiEnemyInfo { get; set; } = new();

	[JsonPropertyName("api_get_base_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiGetBaseExp { get; set; } = default!;

	[JsonPropertyName("api_get_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiGetExp { get; set; } = default!;

	[JsonPropertyName("api_get_exp_lvup")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[JsonPropertyName("api_get_ship_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiGetShipExp { get; set; } = new();

	[JsonPropertyName("api_member_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMemberExp { get; set; } = default!;

	[JsonPropertyName("api_member_lv")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMemberLv { get; set; } = default!;

	[JsonPropertyName("api_mvp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMvp { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_win_rank")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiWinRank { get; set; } = default!;
}
