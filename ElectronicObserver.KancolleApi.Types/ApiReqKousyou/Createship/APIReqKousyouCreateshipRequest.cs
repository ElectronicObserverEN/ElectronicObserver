namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createship;

public class ApiReqKousyouCreateshipRequest
{
	[JsonPropertyName("api_highspeed")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiHighspeed { get; set; } = default!;

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

	[JsonPropertyName("api_item5")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiItem5 { get; set; } = default!;

	[JsonPropertyName("api_kdock_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiKdockId { get; set; } = default!;

	[JsonPropertyName("api_large_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiLargeFlag { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
