namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMaparea
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; }
}
