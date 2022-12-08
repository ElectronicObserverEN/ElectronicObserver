using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createitem;

public class ApiReqKousyouCreateitemResponse
{
	[JsonPropertyName("api_create_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCreateFlag { get; set; } = default!;

	[JsonPropertyName("api_get_items")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiGetItem> ApiGetItems { get; set; } = new();

	[JsonPropertyName("api_material")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiMaterial { get; set; } = new();

	[JsonPropertyName("api_unset_items")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<ApiUnsetItem> ApiUnsetItems { get; set; } = new();
}
