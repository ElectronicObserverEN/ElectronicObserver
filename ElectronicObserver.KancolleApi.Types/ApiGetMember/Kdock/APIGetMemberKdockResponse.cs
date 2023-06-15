namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;

public class ApiGetMemberKdockResponse
{
	[JsonPropertyName("api_complete_time")]
	public long ApiCompleteTime { get; set; } = default!;

	[JsonPropertyName("api_complete_time_str")]
	public string ApiCompleteTimeStr { get; set; } = default!;

	[JsonPropertyName("api_created_ship_id")]
	public int ApiCreatedShipId { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_item1")]
	public int ApiItem1 { get; set; } = default!;

	[JsonPropertyName("api_item2")]
	public int ApiItem2 { get; set; } = default!;

	[JsonPropertyName("api_item3")]
	public int ApiItem3 { get; set; } = default!;

	[JsonPropertyName("api_item4")]
	public int ApiItem4 { get; set; } = default!;

	[JsonPropertyName("api_item5")]
	public int ApiItem5 { get; set; } = default!;

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; } = default!;
}
