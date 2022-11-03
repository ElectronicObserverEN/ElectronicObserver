using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiPort.Port.Response;

public class ApiLog
{

	[JsonPropertyName("api_message")]
	public string? ApiMessage { get; set; }

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; }

	[JsonPropertyName("api_state")]
	public string? ApiState { get; set; }

	[JsonPropertyName("api_type")]
	public string? ApiType { get; set; }

}