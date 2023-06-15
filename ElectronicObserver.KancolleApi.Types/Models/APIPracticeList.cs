namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPracticeList
{
	[JsonPropertyName("api_enemy_comment")]
	public string ApiEnemyComment { get; set; } = "";

	[JsonPropertyName("api_enemy_comment_id")]
	public string ApiEnemyCommentId { get; set; } = "";

	[JsonPropertyName("api_enemy_flag")]
	public int ApiEnemyFlag { get; set; }

	[JsonPropertyName("api_enemy_flag_ship")]
	public int ApiEnemyFlagShip { get; set; }

	[JsonPropertyName("api_enemy_id")]
	public int ApiEnemyId { get; set; }

	[JsonPropertyName("api_enemy_level")]
	public int ApiEnemyLevel { get; set; }

	[JsonPropertyName("api_enemy_name")]
	public string ApiEnemyName { get; set; } = "";

	[JsonPropertyName("api_enemy_name_id")]
	public string ApiEnemyNameId { get; set; } = "";

	[JsonPropertyName("api_enemy_rank")]
	public string ApiEnemyRank { get; set; } = "";

	[JsonPropertyName("api_medals")]
	public int ApiMedals { get; set; }

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; }
}
