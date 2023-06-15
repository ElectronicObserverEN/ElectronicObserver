namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ndock;

public class ApiGetMemberNdockResponse
{
	[JsonPropertyName("api_complete_time")]
	public long ApiCompleteTime { get; set; }

	[JsonPropertyName("api_complete_time_str")]
	public string ApiCompleteTimeStr { get; set; } = "";

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

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; }

	[JsonPropertyName("api_ship_id")]
	public int ApiShipId { get; set; }

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; }
}
