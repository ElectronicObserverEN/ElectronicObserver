namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEscape
{
	[JsonPropertyName("api_escape_idx")]
	public List<int> ApiEscapeIdx { get; set; } = new();

	[JsonPropertyName("api_tow_idx")]
	public List<int> ApiTowIdx { get; set; } = new();
}
