namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.StartAirBase;

public class ApiReqMapStartAirBaseRequest
{
	[System.Text.Json.Serialization.JsonPropertyName("api_strike_point_1")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiStrikePoint1 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_strike_point_2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiStrikePoint2 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_strike_point_3")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiStrikePoint3 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_verno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
