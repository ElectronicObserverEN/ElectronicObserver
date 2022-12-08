namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.Models;

public class ApiEnemyInfo
{
	[JsonPropertyName("api_deck_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckName { get; set; } = default!;

	[JsonPropertyName("api_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_rank")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiRank { get; set; } = default!;

	[JsonPropertyName("api_user_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiUserName { get; set; } = default!;
}
