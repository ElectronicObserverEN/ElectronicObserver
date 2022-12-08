namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiOffshoreSupply
{
	[JsonPropertyName("api_given_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiGivenShip { get; set; } = default!;

	[JsonPropertyName("api_supply_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSupplyShip { get; set; } = default!;

	[JsonPropertyName("api_use_num")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUseNum { get; set; } = default!;
}
