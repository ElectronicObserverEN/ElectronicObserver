namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.ChangeMatchingKind;

public class ApiReqPracticeChangeMatchingKindResponse
{
	[JsonPropertyName("api_update_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUpdateFlag { get; set; } = default!;
}
