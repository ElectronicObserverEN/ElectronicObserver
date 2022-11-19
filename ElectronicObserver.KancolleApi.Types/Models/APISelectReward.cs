namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSelectReward
{
	[JsonPropertyName("api_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCount { get; set; } = default!;

	[JsonPropertyName("api_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiKind { get; set; } = default!;

	[JsonPropertyName("api_mst_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMstId { get; set; } = default!;

	[JsonPropertyName("api_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;
}
