namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.ChangeMatchingKind;

public class ApiReqPracticeChangeMatchingKindRequest
{
	[JsonPropertyName("api_selected_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiSelectedKind { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
