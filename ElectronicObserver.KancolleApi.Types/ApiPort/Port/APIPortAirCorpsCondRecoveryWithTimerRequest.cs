namespace ElectronicObserver.KancolleApi.Types.ApiPort.Port;

public class APIPortAirCorpsCondRecoveryWithTimerRequest
{
	[JsonPropertyName("api_area_id")]
	public string ApiAreaId { get; set; } = "";

	[JsonPropertyName("api_base_id")]
	public string ApiBaseId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
