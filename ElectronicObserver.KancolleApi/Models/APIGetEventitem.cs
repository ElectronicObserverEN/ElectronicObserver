namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiGetEventitem
{
	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_slot_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiSlotLevel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiType { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_value")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiValue { get; set; } = default!;
}
