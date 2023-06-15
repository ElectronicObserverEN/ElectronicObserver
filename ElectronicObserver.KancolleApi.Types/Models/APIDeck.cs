namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiDeck
{
	[JsonPropertyName("api_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_name_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiNameId { get; set; } = default!;

	[JsonPropertyName("api_preset_no")]
	public int ApiPresetNo { get; set; } = default!;

	[JsonPropertyName("api_ship")]
	[Required]
	public List<int> ApiShip { get; set; } = new();
}
