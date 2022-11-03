using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi;

public class ApiResponse<T> where T : class
{
	[JsonPropertyName("api_data")]
	public T? ApiData { get; set; }

	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMessage { get; set; } = "";
}
