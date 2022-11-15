namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirFire
{
	[System.Text.Json.Serialization.JsonPropertyName("api_idx")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiIdx { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_kind")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiKind { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_use_items")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiUseItems { get; set; } = new();
}
