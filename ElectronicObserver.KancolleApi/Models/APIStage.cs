namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage
{
	[System.Text.Json.Serialization.JsonPropertyName("api_air_fire")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiAirFire? ApiAirFire { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_e_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiECount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_e_lostcount")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiELostcount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_f_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_f_lostcount")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFLostcount { get; set; } = default!;
}
