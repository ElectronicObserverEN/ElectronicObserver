namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;

// ApiLValue should be arrays but I don't know any easy way to parse form data into C#
public class ApiReqCombinedBattleBattleresultRequest
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

	[JsonPropertyName("api_l_value3[2]")]
	public string ApiLValue32 { get; set; } = "";

	[JsonPropertyName("api_l_value3[3]")]
	public string ApiLValue33 { get; set; } = "";

	[JsonPropertyName("api_l_value3[4]")]
	public string ApiLValue34 { get; set; } = "";

	[JsonPropertyName("api_l_value3[5]")]
	public string ApiLValue35 { get; set; } = "";

	[JsonPropertyName("api_l_value4[0]")]
	public string ApiLValue40 { get; set; } = "";

	[JsonPropertyName("api_l_value4[1]")]
	public string ApiLValue41 { get; set; } = "";

	[JsonPropertyName("api_l_value4[2]")]
	public string ApiLValue42 { get; set; } = "";

	[JsonPropertyName("api_l_value4[3]")]
	public string ApiLValue43 { get; set; } = "";

	[JsonPropertyName("api_l_value4[4]")]
	public string ApiLValue44 { get; set; } = "";

	[JsonPropertyName("api_l_value4[5]")]
	public string ApiLValue45 { get; set; } = "";
}
