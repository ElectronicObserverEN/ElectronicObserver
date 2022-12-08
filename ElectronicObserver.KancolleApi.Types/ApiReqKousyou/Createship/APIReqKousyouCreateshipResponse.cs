namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createship;

public class ApiReqKousyouCreateshipResponse
{
	[JsonPropertyName("api_result")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiResultMsg { get; set; } = default!;
}
