namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Models;

public class ApiGetItem
{
	[JsonPropertyName("api_useitem_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUseitemCount { get; set; } = default!;

	[JsonPropertyName("api_useitem_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUseitemId { get; set; } = default!;

	[JsonPropertyName("api_useitem_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiUseitemName { get; set; } = default!;
}
