namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiGetEventitem
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_slot_level")]
	public int? ApiSlotLevel { get; set; }

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; }

	[JsonPropertyName("api_value")]
	public int ApiValue { get; set; }
}
