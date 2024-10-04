namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.ExpandBase;

public class ApiReqAirCorpsExpandBaseRequest
{
	[JsonPropertyName("api_token")]
	public required string ApiToken { get; set; }

	[JsonPropertyName("api_verno")]
	public required string ApiVerno { get; set; }

	[JsonPropertyName("api_area_id")]
	public required string ApiAreaId { get; set; }
}
