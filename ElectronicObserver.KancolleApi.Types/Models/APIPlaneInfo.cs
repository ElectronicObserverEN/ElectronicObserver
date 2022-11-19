namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPlaneInfo
{
	[JsonPropertyName("api_cond")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiCond { get; set; } = default!;

	[JsonPropertyName("api_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiCount { get; set; } = default!;

	[JsonPropertyName("api_max_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiMaxCount { get; set; } = default!;

	[JsonPropertyName("api_slotid")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSlotid { get; set; } = default!;

	[JsonPropertyName("api_squadron_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSquadronId { get; set; } = default!;

	[JsonPropertyName("api_state")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiState { get; set; } = default!;
}
