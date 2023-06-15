namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;

public class ApiReqSortieBattleresultRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_btime")]
	[Required(AllowEmptyStrings = true)]
	public string ApiBtime { get; set; } = default!;

	[JsonPropertyName("api_l_value[0]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue0 { get; set; } = default!;

	[JsonPropertyName("api_l_value[1]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue1 { get; set; } = default!;

	[JsonPropertyName("api_l_value[2]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue2 { get; set; } = default!;

	[JsonPropertyName("api_l_value[3]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue3 { get; set; } = default!;

	[JsonPropertyName("api_l_value[4]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue4 { get; set; } = default!;

	[JsonPropertyName("api_l_value[5]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue5 { get; set; } = default!;

	[JsonPropertyName("api_l_value3[0]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue30 { get; set; } = default!;

	[JsonPropertyName("api_l_value3[1]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue31 { get; set; } = default!;
}
