namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPracticeList
{
	[JsonPropertyName("api_enemy_comment")]
	[Required(AllowEmptyStrings = true)]
	public string ApiEnemyComment { get; set; } = default!;

	[JsonPropertyName("api_enemy_comment_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiEnemyCommentId { get; set; } = default!;

	[JsonPropertyName("api_enemy_flag")]
	public int ApiEnemyFlag { get; set; } = default!;

	[JsonPropertyName("api_enemy_flag_ship")]
	public int ApiEnemyFlagShip { get; set; } = default!;

	[JsonPropertyName("api_enemy_id")]
	public int ApiEnemyId { get; set; } = default!;

	[JsonPropertyName("api_enemy_level")]
	public int ApiEnemyLevel { get; set; } = default!;

	[JsonPropertyName("api_enemy_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiEnemyName { get; set; } = default!;

	[JsonPropertyName("api_enemy_name_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiEnemyNameId { get; set; } = default!;

	[JsonPropertyName("api_enemy_rank")]
	[Required(AllowEmptyStrings = true)]
	public string ApiEnemyRank { get; set; } = default!;

	[JsonPropertyName("api_medals")]
	public int ApiMedals { get; set; } = default!;

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; } = default!;
}
