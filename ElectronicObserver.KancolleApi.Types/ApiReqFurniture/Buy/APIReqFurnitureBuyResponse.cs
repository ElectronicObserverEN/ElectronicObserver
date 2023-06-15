namespace ElectronicObserver.KancolleApi.Types.ApiReqFurniture.Buy;

public class ApiReqFurnitureBuyResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = default!;
}
