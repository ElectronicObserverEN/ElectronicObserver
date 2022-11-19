namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEventmap
{
	[JsonPropertyName("api_max_maphp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiMaxMaphp { get; set; } = default!;

	[JsonPropertyName("api_now_maphp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiNowMaphp { get; set; } = default!;

	[JsonPropertyName("api_selected_rank")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSelectedRank { get; set; } = default!;

	[JsonPropertyName("api_state")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiState { get; set; } = default!;

}
