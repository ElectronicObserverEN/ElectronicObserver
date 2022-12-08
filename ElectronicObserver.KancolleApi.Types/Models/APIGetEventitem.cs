namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiGetEventitem
{
	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_slot_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiSlotLevel { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiType { get; set; } = default!;

	[JsonPropertyName("api_value")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiValue { get; set; } = default!;
}
