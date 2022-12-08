namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPracticeList
{
	[JsonPropertyName("api_enemy_comment")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiEnemyComment { get; set; } = default!;

	[JsonPropertyName("api_enemy_comment_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiEnemyCommentId { get; set; } = default!;

	[JsonPropertyName("api_enemy_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiEnemyFlag { get; set; } = default!;

	[JsonPropertyName("api_enemy_flag_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiEnemyFlagShip { get; set; } = default!;

	[JsonPropertyName("api_enemy_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiEnemyId { get; set; } = default!;

	[JsonPropertyName("api_enemy_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiEnemyLevel { get; set; } = default!;

	[JsonPropertyName("api_enemy_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiEnemyName { get; set; } = default!;

	[JsonPropertyName("api_enemy_name_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiEnemyNameId { get; set; } = default!;

	[JsonPropertyName("api_enemy_rank")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiEnemyRank { get; set; } = default!;

	[JsonPropertyName("api_medals")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMedals { get; set; } = default!;

	[JsonPropertyName("api_state")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiState { get; set; } = default!;
}
