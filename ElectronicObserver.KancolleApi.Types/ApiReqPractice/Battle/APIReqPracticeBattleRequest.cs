namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.Battle;

public class ApiReqPracticeBattleRequest
{
	[JsonPropertyName("api_deck_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_enemy_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiEnemyId { get; set; } = default!;

	[JsonPropertyName("api_formation_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiFormationId { get; set; } = default!;

	[JsonPropertyName("api_start")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiStart { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
