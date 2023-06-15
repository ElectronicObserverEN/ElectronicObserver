namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiItem
{
	[JsonPropertyName("api_getmes")]
	public string ApiGetmes { get; set; } = "";

	[JsonPropertyName("api_mode")]
	public int ApiMode { get; set; }

	[JsonPropertyName("api_mst_id")]
	public int ApiMstId { get; set; }

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; }
}
