namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPlaneInfo
{
	[JsonPropertyName("api_cond")]
	public int? ApiCond { get; set; } = default!;

	[JsonPropertyName("api_count")]
	public int? ApiCount { get; set; } = default!;

	[JsonPropertyName("api_max_count")]
	public int? ApiMaxCount { get; set; } = default!;

	[JsonPropertyName("api_slotid")]
	public int ApiSlotid { get; set; } = default!;

	[JsonPropertyName("api_squadron_id")]
	public int ApiSquadronId { get; set; } = default!;

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; } = default!;
}
