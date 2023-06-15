namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.CanPresetSlotSelect;

public class ApiReqKaisouCanPresetSlotSelectRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
