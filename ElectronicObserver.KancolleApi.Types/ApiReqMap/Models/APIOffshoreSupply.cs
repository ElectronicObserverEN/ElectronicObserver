namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiOffshoreSupply
{
	[JsonPropertyName("api_given_ship")]
	public int ApiGivenShip { get; set; } = default!;

	[JsonPropertyName("api_supply_ship")]
	public int ApiSupplyShip { get; set; } = default!;

	[JsonPropertyName("api_use_num")]
	public int ApiUseNum { get; set; } = default!;
}
