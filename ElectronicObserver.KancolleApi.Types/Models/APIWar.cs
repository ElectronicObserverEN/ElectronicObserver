namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiWar
{
	[JsonPropertyName("api_lose")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiLose { get; set; } = default!;

	[JsonPropertyName("api_rate")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiRate { get; set; } = default!;

	[JsonPropertyName("api_win")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiWin { get; set; } = default!;
}
