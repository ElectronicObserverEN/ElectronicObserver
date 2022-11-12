namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Models;

public class ApiGetItem
{
	[System.Text.Json.Serialization.JsonPropertyName("api_useitem_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiUseitemCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_useitem_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiUseitemId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_useitem_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public string? ApiUseitemName { get; set; } = default!;
}
