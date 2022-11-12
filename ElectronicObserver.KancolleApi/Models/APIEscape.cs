namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEscape
{
	[System.Text.Json.Serialization.JsonPropertyName("api_escape_idx")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEscapeIdx { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_tow_idx")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiTowIdx { get; set; } = new();
}
