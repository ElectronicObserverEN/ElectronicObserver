namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiItemget
{
	[JsonPropertyName("api_getcount")]
	public int ApiGetcount { get; set; }

	[JsonPropertyName("api_icon_id")]
	public int ApiIconId { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_usemst")]
	public int ApiUsemst { get; set; }
}
