namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;

public class ApiGetMemberKdockResponse
{
	[JsonPropertyName("api_complete_time")]
	public long ApiCompleteTime { get; set; }

	[JsonPropertyName("api_complete_time_str")]
	public string ApiCompleteTimeStr { get; set; } = "";

	[JsonPropertyName("api_created_ship_id")]
	public int ApiCreatedShipId { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_item1")]
	public int ApiItem1 { get; set; }

	[JsonPropertyName("api_item2")]
	public int ApiItem2 { get; set; }

	[JsonPropertyName("api_item3")]
	public int ApiItem3 { get; set; }

	[JsonPropertyName("api_item4")]
	public int ApiItem4 { get; set; }

	[JsonPropertyName("api_item5")]
	public int ApiItem5 { get; set; }

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; }
}
