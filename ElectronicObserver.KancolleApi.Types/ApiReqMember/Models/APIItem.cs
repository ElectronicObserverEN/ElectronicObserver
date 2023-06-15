namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiItem
{
	[JsonPropertyName("api_getmes")]
	[Required(AllowEmptyStrings = true)]
	public string ApiGetmes { get; set; } = default!;

	[JsonPropertyName("api_mode")]
	public int ApiMode { get; set; } = default!;

	[JsonPropertyName("api_mst_id")]
	public int ApiMstId { get; set; } = default!;

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; } = default!;
}
