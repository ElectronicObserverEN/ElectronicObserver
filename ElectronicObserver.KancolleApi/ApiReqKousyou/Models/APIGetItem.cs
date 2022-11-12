namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

public class ApiGetItem
{
	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_slotitem_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSlotitemId { get; set; } = default!;
}
