namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Change;

public class ApiReqHenseiChangeRequest
{
	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = "";

	[JsonPropertyName("api_ship_id")]
	public string ApiShipId { get; set; } = "";

	[JsonPropertyName("api_ship_idx")]
	public string ApiShipIdx { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
