namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSelectReward
{
	[JsonPropertyName("api_count")]
	public int ApiCount { get; set; }

	[JsonPropertyName("api_kind")]
	public int ApiKind { get; set; }

	[JsonPropertyName("api_mst_id")]
	public int ApiMstId { get; set; }

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; }
}
