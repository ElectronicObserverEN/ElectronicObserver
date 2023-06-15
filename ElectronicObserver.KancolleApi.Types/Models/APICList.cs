namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApicList
{
	[JsonPropertyName("api_c_flag")]
	public int? ApiCFlag { get; set; }

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; }

	[JsonPropertyName("api_progress_flag")]
	public int ApiProgressFlag { get; set; }

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; }
}
