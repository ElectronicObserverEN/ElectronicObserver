namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.SetOssCondition;

public class ApiReqMemberSetOssConditionRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_language_type")]
	public string ApiLanguageType { get; set; } = default!;

	[JsonPropertyName("api_oss_items[0]")]
	public string ApiOssItems0 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[1]")]
	public string ApiOssItems1 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[2]")]
	public string ApiOssItems2 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[3]")]
	public string ApiOssItems3 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[4]")]
	public string ApiOssItems4 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[5]")]
	public string ApiOssItems5 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[6]")]
	public string ApiOssItems6 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[7]")]
	public string ApiOssItems7 { get; set; } = default!;
}
