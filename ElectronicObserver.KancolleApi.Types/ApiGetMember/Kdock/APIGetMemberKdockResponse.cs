namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;

public class ApiGetMemberKdockResponse
{
	[JsonPropertyName("api_complete_time")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public long ApiCompleteTime { get; set; } = default!;

	[JsonPropertyName("api_complete_time_str")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiCompleteTimeStr { get; set; } = default!;

	[JsonPropertyName("api_created_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCreatedShipId { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_item1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiItem1 { get; set; } = default!;

	[JsonPropertyName("api_item2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiItem2 { get; set; } = default!;

	[JsonPropertyName("api_item3")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiItem3 { get; set; } = default!;

	[JsonPropertyName("api_item4")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiItem4 { get; set; } = default!;

	[JsonPropertyName("api_item5")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiItem5 { get; set; } = default!;

	[JsonPropertyName("api_state")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiState { get; set; } = default!;
}
