using ElectronicObserver.KancolleApi.Types.ApiReqPractice.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;

public class ApiReqPracticeBattleResultResponse
{
	[JsonPropertyName("api_enemy_info")]
	public ApiPracticeEnemyInfo ApiEnemyInfo { get; set; } = new();

	[JsonPropertyName("api_get_base_exp")]
	public int ApiGetBaseExp { get; set; }

	[JsonPropertyName("api_get_exp")]
	public int ApiGetExp { get; set; }

	[JsonPropertyName("api_get_exp_lvup")]
	public List<List<int>> ApiGetExpLvup { get; set; } = new();

	[JsonPropertyName("api_get_ship_exp")]
	public List<int> ApiGetShipExp { get; set; } = new();

	[JsonPropertyName("api_member_exp")]
	public int ApiMemberExp { get; set; }

	[JsonPropertyName("api_member_lv")]
	public int ApiMemberLv { get; set; }

	[JsonPropertyName("api_mvp")]
	public int ApiMvp { get; set; }

	[JsonPropertyName("api_ship_id")]
	public List<int> ApiShipId { get; set; } = new();

	// todo: should be an enum
	[JsonPropertyName("api_win_rank")]
	public string ApiWinRank { get; set; } = "";
}
