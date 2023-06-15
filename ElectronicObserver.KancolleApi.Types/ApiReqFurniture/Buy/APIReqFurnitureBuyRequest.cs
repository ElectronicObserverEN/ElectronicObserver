namespace ElectronicObserver.KancolleApi.Types.ApiReqFurniture.Buy;

public class ApiReqFurnitureBuyRequest
{
	[JsonPropertyName("api_no")]
	[Required(AllowEmptyStrings = true)]
	public string ApiNo { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[Required(AllowEmptyStrings = true)]
	public string ApiType { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
