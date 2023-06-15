namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Lock;

public class ApiReqHenseiLockRequest
{
	[JsonPropertyName("api_ship_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
