namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEnemyInfo
{
	[JsonPropertyName("api_deck_name")]
	public string ApiDeckName { get; set; } = "";

	[JsonPropertyName("api_level")]
	public string ApiLevel { get; set; } = "";

	[JsonPropertyName("api_rank")]
	public string ApiRank { get; set; } = "";

}
