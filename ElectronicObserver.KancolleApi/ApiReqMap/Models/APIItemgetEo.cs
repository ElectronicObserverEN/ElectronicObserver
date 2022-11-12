namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiItemgetEo
{
	[System.Text.Json.Serialization.JsonPropertyName("api_getcount")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiGetcount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_usemst")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiUsemst { get; set; } = default!;
}
