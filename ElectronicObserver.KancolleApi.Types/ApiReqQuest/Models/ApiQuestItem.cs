using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Models;

public class ApiQuestItem
{
	[JsonPropertyName("api_id")]
	public UseItemId? ApiId { get; set; }

	[JsonPropertyName("api_id_from")]
	public int? ApiIdFrom { get; set; }

	[JsonPropertyName("api_id_to")]
	public int? ApiIdTo { get; set; }

	[JsonPropertyName("api_message")]
	public string? ApiMessage { get; set; }

	[JsonPropertyName("api_name")]
	public string? ApiName { get; set; }
}
