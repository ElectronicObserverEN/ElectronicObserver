namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiSlotitem
{
	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_locked")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLocked { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_slotitem_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSlotitemId { get; set; } = default!;
}
