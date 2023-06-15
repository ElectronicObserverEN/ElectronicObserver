namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiWar
{
	[JsonPropertyName("api_lose")]
	[Required(AllowEmptyStrings = true)]
	public string ApiLose { get; set; } = default!;

	[JsonPropertyName("api_rate")]
	[Required(AllowEmptyStrings = true)]
	public string ApiRate { get; set; } = default!;

	[JsonPropertyName("api_win")]
	[Required(AllowEmptyStrings = true)]
	public string ApiWin { get; set; } = default!;
}
