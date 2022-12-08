namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createitem;

public class ApiReqKousyouCreateitemRequest
{
	[JsonPropertyName("api_item1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiItem1 { get; set; } = default!;

	[JsonPropertyName("api_item2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiItem2 { get; set; } = default!;

	[JsonPropertyName("api_item3")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiItem3 { get; set; } = default!;

	[JsonPropertyName("api_item4")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiItem4 { get; set; } = default!;

	[JsonPropertyName("api_multiple_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiMultipleFlag { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
