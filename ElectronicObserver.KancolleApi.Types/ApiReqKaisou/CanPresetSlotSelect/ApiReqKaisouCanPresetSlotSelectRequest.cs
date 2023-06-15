namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.CanPresetSlotSelect;

public class ApiReqKaisouCanPresetSlotSelectRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
