namespace ElectronicObserver.KancolleApi.Types.ApiReqFurniture.Buy;

public class ApiReqFurnitureBuyRequest
{
	[JsonPropertyName("api_no")]
	public string ApiNo { get; set; } = "";

	[JsonPropertyName("api_type")]
	public string ApiType { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
