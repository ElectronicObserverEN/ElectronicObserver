namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFurniture
{
	[System.Text.Json.Serialization.JsonPropertyName("api_furniture_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFurnitureId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_furniture_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFurnitureNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_furniture_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFurnitureType { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;
}
