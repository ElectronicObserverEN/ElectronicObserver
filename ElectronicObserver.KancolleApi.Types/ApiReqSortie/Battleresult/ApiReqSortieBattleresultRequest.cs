namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;

public class ApiReqSortieBattleresultRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("api_btime")]
	public string ApiBtime { get; set; } = "";

	[JsonPropertyName("api_l_value[0]")]
	public string ApiLValue0 { get; set; } = "";

	[JsonPropertyName("api_l_value[1]")]
	public string ApiLValue1 { get; set; } = "";

	[JsonPropertyName("api_l_value[2]")]
	public string ApiLValue2 { get; set; } = "";

	[JsonPropertyName("api_l_value[3]")]
	public string ApiLValue3 { get; set; } = "";

	[JsonPropertyName("api_l_value[4]")]
	public string ApiLValue4 { get; set; } = "";

	[JsonPropertyName("api_l_value[5]")]
	public string ApiLValue5 { get; set; } = "";

	[JsonPropertyName("api_l_value3[0]")]
	public string ApiLValue30 { get; set; } = "";

	[JsonPropertyName("api_l_value3[1]")]
	public string ApiLValue31 { get; set; } = "";
}
