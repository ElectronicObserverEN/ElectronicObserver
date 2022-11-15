using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createitem;

public class ApiReqKousyouCreateitemResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_create_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCreateFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_get_items")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiGetItem> ApiGetItems { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_material")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiMaterial { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_unset_items")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiUnsetItem? ApiUnsetItems { get; set; } = default!;
}
