namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiWar
{
	[JsonPropertyName("api_lose")]
	public string ApiLose { get; set; } = "";

	[JsonPropertyName("api_rate")]
	public string ApiRate { get; set; } = "";

	[JsonPropertyName("api_win")]
	public string ApiWin { get; set; } = "";
}
