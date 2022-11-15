namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiGetUseitem
{
	[System.Text.Json.Serialization.JsonPropertyName("api_useitem_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiUseitemId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_useitem_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiUseitemName { get; set; } = default!;
}
