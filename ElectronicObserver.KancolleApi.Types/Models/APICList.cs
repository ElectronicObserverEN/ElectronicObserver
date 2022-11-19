namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApicList
{
	[JsonPropertyName("api_c_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiCFlag { get; set; } = default!;

	[JsonPropertyName("api_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_progress_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiProgressFlag { get; set; } = default!;

	[JsonPropertyName("api_state")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiState { get; set; } = default!;
}
