namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createitem;

public class ApiReqKousyouCreateitemRequest
{
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

	[JsonPropertyName("api_multiple_flag")]
	public string? ApiMultipleFlag { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
