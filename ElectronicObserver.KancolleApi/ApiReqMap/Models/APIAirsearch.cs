namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiAirsearch
{
	[System.Text.Json.Serialization.JsonPropertyName("api_plane_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiPlaneType { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_result")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiResult { get; set; } = default!;
}
