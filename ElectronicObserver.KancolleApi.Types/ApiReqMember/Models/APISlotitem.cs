namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiSlotitem
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_locked")]
	public int ApiLocked { get; set; } = default!;

	[JsonPropertyName("api_slotitem_id")]
	public int ApiSlotitemId { get; set; } = default!;
}
