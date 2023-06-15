namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiDistance
{
	[JsonPropertyName("api_base")]
	public int ApiBase { get; set; }

	[JsonPropertyName("api_bonus")]
	public int ApiBonus { get; set; }
}
