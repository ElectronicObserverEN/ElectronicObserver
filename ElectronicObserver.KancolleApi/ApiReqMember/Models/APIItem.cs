namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiItem
{
	[System.Text.Json.Serialization.JsonPropertyName("api_getmes")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiGetmes { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_mode")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMode { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMstId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiType { get; set; } = default!;
}
