﻿namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;

// ApiLValue should be arrays but I don't know any easy way to parse form data into C#
public class ApiReqCombinedBattleBattleresultRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; }

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; }

	[JsonPropertyName("api_btime")]
	[Required(AllowEmptyStrings = true)]
	public string ApiBtime { get; set; }

	[JsonPropertyName("api_l_value[0]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue0 { get; set; }

	[JsonPropertyName("api_l_value[1]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue1 { get; set; }

	[JsonPropertyName("api_l_value[2]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue2 { get; set; }

	[JsonPropertyName("api_l_value[3]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue3 { get; set; }

	[JsonPropertyName("api_l_value[4]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue4 { get; set; }

	[JsonPropertyName("api_l_value[5]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue5 { get; set; }

	[JsonPropertyName("api_l_value3[0]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue30 { get; set; }

	[JsonPropertyName("api_l_value3[1]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue31 { get; set; }

	[JsonPropertyName("api_l_value3[2]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue32 { get; set; }

	[JsonPropertyName("api_l_value3[3]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue33 { get; set; }

	[JsonPropertyName("api_l_value3[4]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue34 { get; set; }

	[JsonPropertyName("api_l_value3[5]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue35 { get; set; }

	[JsonPropertyName("api_l_value4[0]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue40 { get; set; }

	[JsonPropertyName("api_l_value4[1]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue41 { get; set; }

	[JsonPropertyName("api_l_value4[2]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue42 { get; set; }

	[JsonPropertyName("api_l_value4[3]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue43 { get; set; }

	[JsonPropertyName("api_l_value4[4]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue44 { get; set; }

	[JsonPropertyName("api_l_value4[5]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLValue45 { get; set; }
}
