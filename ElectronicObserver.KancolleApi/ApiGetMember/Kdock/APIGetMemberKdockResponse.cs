namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;

public class ApiGetMemberKdockResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_complete_time")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCompleteTime { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_complete_time_str")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiCompleteTimeStr { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_created_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCreatedShipId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item1")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiItem1 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiItem2 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item3")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiItem3 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item4")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiItem4 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item5")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiItem5 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_state")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiState { get; set; } = default!;
}
