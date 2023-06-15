using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiGetItemList
{
	[JsonPropertyName("api_item_no")]
	[Required]
	public int ApiItemNo { get; set; }

	[JsonPropertyName("api_type")]
	[Required]
	public int ApiType { get; set; }

	[JsonPropertyName("api_id")]
	[Required]
	public int ApiId { get; set; }

	[JsonPropertyName("api_value")]
	[Required]
	public int ApiValue { get; set; }
}
