namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Lock;

public class ApiReqHenseiLockRequest
{
	[JsonPropertyName("api_ship_id")]
	public string ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
