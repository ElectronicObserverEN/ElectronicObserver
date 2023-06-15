namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEnemyInfo
{
	[JsonPropertyName("api_deck_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckName { get; set; } = default!;

	[JsonPropertyName("api_level")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_rank")]
	[Required(AllowEmptyStrings = true)]
	public string ApiRank { get; set; } = default!;

}
