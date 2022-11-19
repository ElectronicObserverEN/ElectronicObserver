namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

public class ApiAfterSlot
{
	[JsonPropertyName("api_alv")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiAlv { get; set; } = default!;

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
