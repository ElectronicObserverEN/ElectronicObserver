namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Models;

public class ApiGetItem
{
	[JsonPropertyName("api_useitem_count")]
	public int ApiUseitemCount { get; set; } = default!;

	[JsonPropertyName("api_useitem_id")]
	public int ApiUseitemId { get; set; } = default!;

	[JsonPropertyName("api_useitem_name")]
	public string? ApiUseitemName { get; set; } = default!;
}
