namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApicList
{
	[JsonPropertyName("api_c_flag")]
	public int? ApiCFlag { get; set; } = default!;

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_progress_flag")]
	public int ApiProgressFlag { get; set; } = default!;

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; } = default!;
}
