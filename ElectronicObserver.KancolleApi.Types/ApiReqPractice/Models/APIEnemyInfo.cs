namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.Models;

public class ApiEnemyInfo
{
	[JsonPropertyName("api_deck_name")]
	public string ApiDeckName { get; set; } = default!;

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_rank")]
	public string ApiRank { get; set; } = default!;

	[JsonPropertyName("api_user_name")]
	public string ApiUserName { get; set; } = default!;
}
