namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPlaneInfo
{
	[JsonPropertyName("api_cond")]
	public int? ApiCond { get; set; }

	[JsonPropertyName("api_count")]
	public int? ApiCount { get; set; }

	[JsonPropertyName("api_max_count")]
	public int? ApiMaxCount { get; set; }

	[JsonPropertyName("api_slotid")]
	public int ApiSlotid { get; set; }

	[JsonPropertyName("api_squadron_id")]
	public int ApiSquadronId { get; set; }

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; }
}
