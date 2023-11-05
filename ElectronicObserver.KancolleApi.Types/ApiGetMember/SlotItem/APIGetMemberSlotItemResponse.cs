namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.SlotItem;

public class ApiGetMemberSlotItemResponse
{
	[JsonPropertyName("api_alv")]
	public int? ApiAlv { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; }

	[JsonPropertyName("api_locked")]
	public int ApiLocked { get; set; }

	[JsonPropertyName("api_slotitem_id")]
	public int ApiSlotitemId { get; set; }
}
