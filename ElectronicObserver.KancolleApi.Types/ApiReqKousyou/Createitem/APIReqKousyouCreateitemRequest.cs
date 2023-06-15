namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createitem;

public class ApiReqKousyouCreateitemRequest
{
	[JsonPropertyName("api_item1")]
	public string ApiItem1 { get; set; } = "";

	[JsonPropertyName("api_item2")]
	public string ApiItem2 { get; set; } = "";

	[JsonPropertyName("api_item3")]
	public string ApiItem3 { get; set; } = "";

	[JsonPropertyName("api_item4")]
	public string ApiItem4 { get; set; } = "";

	[JsonPropertyName("api_multiple_flag")]
	public string? ApiMultipleFlag { get; set; }

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
