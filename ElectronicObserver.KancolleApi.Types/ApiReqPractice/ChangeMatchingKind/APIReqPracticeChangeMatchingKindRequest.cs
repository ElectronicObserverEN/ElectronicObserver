namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.ChangeMatchingKind;

public class ApiReqPracticeChangeMatchingKindRequest
{
	[JsonPropertyName("api_selected_kind")]
	public string ApiSelectedKind { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
