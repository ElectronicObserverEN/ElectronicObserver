namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiDistance
{
	[JsonPropertyName("api_base")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBase { get; set; } = default!;

	[JsonPropertyName("api_bonus")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBonus { get; set; } = default!;
}
