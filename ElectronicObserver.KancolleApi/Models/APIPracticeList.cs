namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPracticeList
{
	[System.Text.Json.Serialization.JsonPropertyName("api_enemy_comment")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiEnemyComment { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_enemy_comment_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiEnemyCommentId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_enemy_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiEnemyFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_enemy_flag_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiEnemyFlagShip { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_enemy_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiEnemyId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_enemy_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiEnemyLevel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_enemy_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiEnemyName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_enemy_name_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiEnemyNameId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_enemy_rank")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiEnemyRank { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_medals")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMedals { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_state")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiState { get; set; } = default!;
}
