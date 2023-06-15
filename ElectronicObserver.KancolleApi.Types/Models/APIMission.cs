namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiMission
{
	[JsonPropertyName("api_count")]
	public string ApiCount { get; set; } = default!;

	[JsonPropertyName("api_rate")]
	public string ApiRate { get; set; } = default!;

	[JsonPropertyName("api_success")]
	public string ApiSuccess { get; set; } = default!;
}
