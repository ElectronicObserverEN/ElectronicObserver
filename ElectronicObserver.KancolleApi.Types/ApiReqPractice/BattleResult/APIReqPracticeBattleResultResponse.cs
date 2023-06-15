using ElectronicObserver.KancolleApi.Types.ApiReqPractice.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;

public class ApiReqPracticeBattleResultResponse
{
	[JsonPropertyName("api_enemy_info")]
	[Required]
	public ApiEnemyInfo ApiEnemyInfo { get; set; } = new();

	[JsonPropertyName("api_get_base_exp")]
	public int ApiGetBaseExp { get; set; } = default!;

	[JsonPropertyName("api_get_exp")]
	public int ApiGetExp { get; set; } = default!;

	[JsonPropertyName("api_get_exp_lvup")]
	[Required]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[JsonPropertyName("api_get_ship_exp")]
	[Required]
	public List<int> ApiGetShipExp { get; set; } = new();

	[JsonPropertyName("api_member_exp")]
	public int ApiMemberExp { get; set; } = default!;

	[JsonPropertyName("api_member_lv")]
	public int ApiMemberLv { get; set; } = default!;

	[JsonPropertyName("api_mvp")]
	public int ApiMvp { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[Required]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_win_rank")]
	[Required(AllowEmptyStrings = true)]
	public string ApiWinRank { get; set; } = default!;
}
