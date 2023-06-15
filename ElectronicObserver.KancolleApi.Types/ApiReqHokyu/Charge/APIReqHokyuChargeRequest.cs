namespace ElectronicObserver.KancolleApi.Types.ApiReqHokyu.Charge;

public class ApiReqHokyuChargeRequest
{
	[JsonPropertyName("api_id_items")]
	[Required(AllowEmptyStrings = true)]
	public string ApiIdItems { get; set; } = default!;

	[JsonPropertyName("api_kind")]
	[Required(AllowEmptyStrings = true)]
	public string ApiKind { get; set; } = default!;

	[JsonPropertyName("api_onslot")]
	[Required(AllowEmptyStrings = true)]
	public string ApiOnslot { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
