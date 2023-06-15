namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiChargeShip
{
	[JsonPropertyName("api_bull")]
	public int ApiBull { get; set; } = default!;

	[JsonPropertyName("api_fuel")]
	public int ApiFuel { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_onslot")]
	public List<int> ApiOnslot { get; set; } = new();
}
