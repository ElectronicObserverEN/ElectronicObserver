namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createship;

public class ApiReqKousyouCreateshipRequest
{
	[JsonPropertyName("api_highspeed")]
	public string ApiHighspeed { get; set; } = "";

	[JsonPropertyName("api_item1")]
	public string ApiItem1 { get; set; } = "";

	[JsonPropertyName("api_item2")]
	public string ApiItem2 { get; set; } = "";

	[JsonPropertyName("api_item3")]
	public string ApiItem3 { get; set; } = "";

	[JsonPropertyName("api_item4")]
	public string ApiItem4 { get; set; } = "";

	[JsonPropertyName("api_item5")]
	public string ApiItem5 { get; set; } = "";

	[JsonPropertyName("api_kdock_id")]
	public string ApiKdockId { get; set; } = "";

	[JsonPropertyName("api_large_flag")]
	public string ApiLargeFlag { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
