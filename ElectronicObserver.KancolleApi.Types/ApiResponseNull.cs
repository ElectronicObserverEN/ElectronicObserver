namespace ElectronicObserver.KancolleApi.Types;

public class ApiResponseNull<T> where T : class, new()
{
	[JsonPropertyName("api_data")]
	public T? ApiData { get; set; } = new();

	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMessage { get; set; } = "";
}
