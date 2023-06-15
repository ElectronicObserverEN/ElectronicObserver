namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiLog
{
	[JsonPropertyName("api_message")]
	public string ApiMessage { get; set; } = default!;

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_state")]
	public string ApiState { get; set; } = default!;

	[JsonPropertyName("api_type")]
	public string ApiType { get; set; } = default!;
}
