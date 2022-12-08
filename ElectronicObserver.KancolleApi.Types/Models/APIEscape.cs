namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEscape
{
	[JsonPropertyName("api_escape_idx")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEscapeIdx { get; set; } = new();

	[JsonPropertyName("api_tow_idx")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiTowIdx { get; set; } = new();
}
