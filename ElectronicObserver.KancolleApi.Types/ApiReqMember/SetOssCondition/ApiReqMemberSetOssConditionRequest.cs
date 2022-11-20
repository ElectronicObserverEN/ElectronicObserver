namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.SetOssCondition;

public class ApiReqMemberSetOssConditionRequest
{
	[JsonPropertyName("api_token")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_language_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiLanguageType { get; set; } = default!;

	[JsonPropertyName("api_oss_items[0]")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems0 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[1]")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems1 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[2]")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems2 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[3]")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems3 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[4]")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems4 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[5]")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems5 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[6]")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems6 { get; set; } = default!;

	[JsonPropertyName("api_oss_items[7]")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiOssItems7 { get; set; } = default!;
}
