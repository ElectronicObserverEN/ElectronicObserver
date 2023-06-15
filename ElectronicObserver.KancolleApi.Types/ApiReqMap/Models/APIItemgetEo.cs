namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiItemgetEo
{
	[JsonPropertyName("api_getcount")]
	public int ApiGetcount { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_usemst")]
	public int ApiUsemst { get; set; } = default!;
}
