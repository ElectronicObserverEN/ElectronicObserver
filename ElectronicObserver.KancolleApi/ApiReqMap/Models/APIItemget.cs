namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiItemget
{
	[System.Text.Json.Serialization.JsonPropertyName("api_getcount")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiGetcount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_icon_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiIconId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_usemst")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiUsemst { get; set; } = default!;
}
