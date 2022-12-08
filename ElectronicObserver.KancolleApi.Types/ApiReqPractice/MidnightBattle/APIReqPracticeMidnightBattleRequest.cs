namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.MidnightBattle;

public class ApiReqPracticeMidnightBattleRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
