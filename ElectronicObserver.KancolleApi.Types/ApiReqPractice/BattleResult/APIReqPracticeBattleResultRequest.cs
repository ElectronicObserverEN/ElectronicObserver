namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;

public class ApiReqPracticeBattleResultRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
