using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createitem;

public class ApiReqKousyouCreateitemResponse
{
	[JsonPropertyName("api_create_flag")]
	public int ApiCreateFlag { get; set; }

	[JsonPropertyName("api_get_items")]
	public List<ApiGetItem> ApiGetItems { get; set; } = new();

	[JsonPropertyName("api_material")]
	public List<int> ApiMaterial { get; set; } = new();

	[JsonPropertyName("api_unset_items")]
	public List<ApiUnsetItem> ApiUnsetItems { get; set; } = new();
}
