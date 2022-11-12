namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiDistance
{
	[System.Text.Json.Serialization.JsonPropertyName("api_base")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiBase { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_bonus")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiBonus { get; set; } = default!;
}
