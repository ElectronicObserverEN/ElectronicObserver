namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiGetItemList
{
	[JsonPropertyName("api_item_no")]
	public int ApiItemNo { get; set; }

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_value")]
	public int ApiValue { get; set; }
}
