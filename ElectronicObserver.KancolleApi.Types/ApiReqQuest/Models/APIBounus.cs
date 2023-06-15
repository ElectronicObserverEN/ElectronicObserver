using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Models;

public class ApiBounus
{
	[JsonPropertyName("api_count")]
	public int ApiCount { get; set; }

	[JsonPropertyName("api_item")]
	public ApiItem? ApiItem { get; set; }

	[JsonPropertyName("api_type")]
	public UseItemId ApiType { get; set; }
}
