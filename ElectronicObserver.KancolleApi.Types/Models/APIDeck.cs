namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiDeck
{
	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_name_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiNameId { get; set; } = default!;

	[JsonPropertyName("api_preset_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPresetNo { get; set; } = default!;

	[JsonPropertyName("api_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShip { get; set; } = new();
}
