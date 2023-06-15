using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Models;

public class ApiItem
{
	[JsonPropertyName("api_id")]
	public UseItemId? ApiId { get; set; } = default!;

	[JsonPropertyName("api_id_from")]
	public int? ApiIdFrom { get; set; } = default!;

	[JsonPropertyName("api_id_to")]
	public int? ApiIdTo { get; set; } = default!;

	[JsonPropertyName("api_message")]
	public string? ApiMessage { get; set; } = default!;

	[JsonPropertyName("api_name")]
	public string? ApiName { get; set; } = default!;
}
