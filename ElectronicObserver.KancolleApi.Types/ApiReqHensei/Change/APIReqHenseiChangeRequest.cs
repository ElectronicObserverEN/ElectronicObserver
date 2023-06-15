namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Change;

public class ApiReqHenseiChangeRequest
{
	[JsonPropertyName("api_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_ship_idx")]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipIdx { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
