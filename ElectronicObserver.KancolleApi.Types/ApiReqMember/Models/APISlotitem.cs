namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiSlotitem
{
	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_locked")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLocked { get; set; } = default!;

	[JsonPropertyName("api_slotitem_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSlotitemId { get; set; } = default!;
}
