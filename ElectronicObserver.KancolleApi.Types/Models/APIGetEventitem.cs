namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiGetEventitem
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_slot_level")]
	public int? ApiSlotLevel { get; set; } = default!;

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; } = default!;

	[JsonPropertyName("api_value")]
	public int ApiValue { get; set; } = default!;
}
