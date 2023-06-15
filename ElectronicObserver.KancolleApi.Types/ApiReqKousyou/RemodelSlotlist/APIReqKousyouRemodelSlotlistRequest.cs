namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlotlist;

public class ApiReqKousyouRemodelSlotlistRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
