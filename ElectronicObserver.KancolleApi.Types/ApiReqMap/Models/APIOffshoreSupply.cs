namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiOffshoreSupply
{
	[System.Text.Json.Serialization.JsonPropertyName("api_given_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiGivenShip { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_supply_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSupplyShip { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_use_num")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiUseNum { get; set; } = default!;
}
