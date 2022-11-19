namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiChargeShip
{
	[JsonPropertyName("api_bull")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBull { get; set; } = default!;

	[JsonPropertyName("api_fuel")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFuel { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_onslot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiOnslot { get; set; } = new();
}
