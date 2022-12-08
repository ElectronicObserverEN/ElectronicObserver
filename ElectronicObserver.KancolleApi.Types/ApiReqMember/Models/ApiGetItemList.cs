using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiGetItemList
{
	[JsonPropertyName("api_item_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiItemNo { get; set; }

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiType { get; set; }

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiId { get; set; }

	[JsonPropertyName("api_value")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiValue { get; set; }
}
