namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.SetOssCondition;

public class ApiReqMemberSetOssConditionRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_language_type")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLanguageType { get; set; } = default!;

	[JsonPropertyName("api_oss_items[0]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems0 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[1]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems1 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[2]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems2 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[3]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems3 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[4]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems4 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[5]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems5 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[6]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems6 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[7]")]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems7 { get; set; } = default!;
}
