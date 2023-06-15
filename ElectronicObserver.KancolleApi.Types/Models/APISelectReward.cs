namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSelectReward
{
	[JsonPropertyName("api_count")]
	public int ApiCount { get; set; } = default!;

	[JsonPropertyName("api_kind")]
	public int ApiKind { get; set; } = default!;

	[JsonPropertyName("api_mst_id")]
	public int ApiMstId { get; set; } = default!;

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; } = default!;
}
