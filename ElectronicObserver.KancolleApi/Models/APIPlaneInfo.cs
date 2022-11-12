namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPlaneInfo
{
	[System.Text.Json.Serialization.JsonPropertyName("api_cond")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiCond { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_max_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiMaxCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_slotid")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSlotid { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_squadron_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSquadronId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_state")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiState { get; set; } = default!;
}
