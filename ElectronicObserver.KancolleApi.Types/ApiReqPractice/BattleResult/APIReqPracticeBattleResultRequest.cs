namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;

public class ApiReqPracticeBattleResultRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
