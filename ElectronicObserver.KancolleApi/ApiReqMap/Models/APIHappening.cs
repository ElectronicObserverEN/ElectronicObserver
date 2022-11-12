namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiHappening
{
	[System.Text.Json.Serialization.JsonPropertyName("api_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_dentan")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiDentan { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_icon_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiIconId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMstId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiType { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_usemst")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiUsemst { get; set; } = default!;
}
