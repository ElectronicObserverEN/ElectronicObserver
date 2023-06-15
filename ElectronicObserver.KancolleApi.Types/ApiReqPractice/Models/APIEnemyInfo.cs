namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.Models;

public class ApiEnemyInfo
{
	[JsonPropertyName("api_deck_name")]
	public string ApiDeckName { get; set; } = "";

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; }

	[JsonPropertyName("api_rank")]
	public string ApiRank { get; set; } = "";

	[JsonPropertyName("api_user_name")]
	public string ApiUserName { get; set; } = "";
}
