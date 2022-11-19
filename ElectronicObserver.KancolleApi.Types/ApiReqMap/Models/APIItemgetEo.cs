namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiItemgetEo
{
	[JsonPropertyName("api_getcount")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiGetcount { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_usemst")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUsemst { get; set; } = default!;
}
