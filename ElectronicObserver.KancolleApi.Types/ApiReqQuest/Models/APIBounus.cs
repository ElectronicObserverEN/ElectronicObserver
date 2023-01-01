using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Models;

public class ApiBounus
{
	[JsonPropertyName("api_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCount { get; set; } = default!;

	[JsonPropertyName("api_item")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiItem? ApiItem { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public UseItemId ApiType { get; set; } = default!;
}
