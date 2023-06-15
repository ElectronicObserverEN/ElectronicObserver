namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.GoBackPort;

public class ApiReqSortieGobackPortRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
