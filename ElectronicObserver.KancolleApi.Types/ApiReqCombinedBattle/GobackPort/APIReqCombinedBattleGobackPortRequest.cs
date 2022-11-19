namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.GobackPort;

public class ApiReqCombinedBattleGobackPortRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
