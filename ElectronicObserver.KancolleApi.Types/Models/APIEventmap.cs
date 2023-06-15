namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEventmap
{
	[JsonPropertyName("api_max_maphp")]
	public int? ApiMaxMaphp { get; set; } = default!;

	[JsonPropertyName("api_now_maphp")]
	public int? ApiNowMaphp { get; set; } = default!;

	[JsonPropertyName("api_selected_rank")]
	public int ApiSelectedRank { get; set; } = default!;

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; } = default!;

}
