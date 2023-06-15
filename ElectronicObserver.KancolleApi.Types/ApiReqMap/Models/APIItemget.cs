namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiItemget
{
	[JsonPropertyName("api_getcount")]
	public int ApiGetcount { get; set; } = default!;

	[JsonPropertyName("api_icon_id")]
	public int ApiIconId { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_usemst")]
	public int ApiUsemst { get; set; } = default!;
}
