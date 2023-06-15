namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createship;

public class ApiReqKousyouCreateshipRequest
{
	[JsonPropertyName("api_highspeed")]
	[Required(AllowEmptyStrings = true)]
	public string ApiHighspeed { get; set; } = default!;

	[JsonPropertyName("api_item1")]
	[Required(AllowEmptyStrings = true)]
	public string ApiItem1 { get; set; } = default!;

	[JsonPropertyName("api_item2")]
	[Required(AllowEmptyStrings = true)]
	public string ApiItem2 { get; set; } = default!;

	[JsonPropertyName("api_item3")]
	[Required(AllowEmptyStrings = true)]
	public string ApiItem3 { get; set; } = default!;

	[JsonPropertyName("api_item4")]
	[Required(AllowEmptyStrings = true)]
	public string ApiItem4 { get; set; } = default!;

	[JsonPropertyName("api_item5")]
	[Required(AllowEmptyStrings = true)]
	public string ApiItem5 { get; set; } = default!;

	[JsonPropertyName("api_kdock_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiKdockId { get; set; } = default!;

	[JsonPropertyName("api_large_flag")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLargeFlag { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
