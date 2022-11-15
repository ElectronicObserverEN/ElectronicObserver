namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage2
{
	[System.Text.Json.Serialization.JsonPropertyName("api_f_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_f_lostcount")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFLostcount { get; set; } = default!;
}
