namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEventmap
{
	[System.Text.Json.Serialization.JsonPropertyName("api_max_maphp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiMaxMaphp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_now_maphp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiNowMaphp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_selected_rank")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSelectedRank { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_state")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiState { get; set; } = default!;

}
