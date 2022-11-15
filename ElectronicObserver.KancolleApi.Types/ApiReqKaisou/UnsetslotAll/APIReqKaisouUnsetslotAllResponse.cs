namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.UnsetslotAll;

public class ApiReqKaisouUnsetslotAllResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_result")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiResult { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_result_msg")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiResultMsg { get; set; } = default!;
}
