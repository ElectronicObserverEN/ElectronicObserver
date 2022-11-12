namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Createship;

public class ApiReqKousyouCreateshipRequest
{
	[System.Text.Json.Serialization.JsonPropertyName("api_highspeed")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiHighspeed { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item1")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiItem1 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiItem2 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item3")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiItem3 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item4")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiItem4 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item5")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiItem5 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_kdock_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiKdockId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_large_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiLargeFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_verno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
