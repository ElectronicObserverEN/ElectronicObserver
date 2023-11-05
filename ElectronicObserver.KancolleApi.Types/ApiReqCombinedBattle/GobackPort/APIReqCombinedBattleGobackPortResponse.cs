namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.GobackPort;

public class ApiReqCombinedBattleGobackPortResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = "";
}
