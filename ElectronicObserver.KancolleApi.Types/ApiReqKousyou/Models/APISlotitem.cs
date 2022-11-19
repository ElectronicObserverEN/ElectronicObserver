namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

public class ApiSlotitem
{
	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_slotitem_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSlotitemId { get; set; } = default!;

}
