namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEnemyInfo
{
	[JsonPropertyName("api_deck_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckName { get; set; } = default!;

	[JsonPropertyName("api_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_rank")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiRank { get; set; } = default!;

}
