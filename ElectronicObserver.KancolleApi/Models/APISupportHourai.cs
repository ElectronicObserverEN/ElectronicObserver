namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSupportHourai
{
	[System.Text.Json.Serialization.JsonPropertyName("api_cl_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiClList { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_damage")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiDamage { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_deck_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiDeckId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipId { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_undressing_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiUndressingFlag { get; set; } = new();
}
