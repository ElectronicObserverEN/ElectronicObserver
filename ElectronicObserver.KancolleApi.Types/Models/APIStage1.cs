namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage1
{
	[System.Text.Json.Serialization.JsonPropertyName("api_disp_seiku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiDispSeiku { get; set; } = default!;

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

	[System.Text.Json.Serialization.JsonPropertyName("api_touch_plane")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiTouchPlane { get; set; } = new();
}
