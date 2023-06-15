namespace ElectronicObserver.KancolleApi.Types.ApiReqHokyu.Charge;

public class ApiReqHokyuChargeRequest
{
	[JsonPropertyName("api_id_items")]
	public string ApiIdItems { get; set; } = default!;

	[JsonPropertyName("api_kind")]
	public string ApiKind { get; set; } = default!;

	[JsonPropertyName("api_onslot")]
	public string ApiOnslot { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
