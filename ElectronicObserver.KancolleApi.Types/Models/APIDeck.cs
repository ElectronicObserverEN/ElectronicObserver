namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiDeck
{
	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_name_id")]
	public string ApiNameId { get; set; } = "";

	[JsonPropertyName("api_preset_no")]
	public int ApiPresetNo { get; set; }

	[JsonPropertyName("api_ship")]
	public List<int> ApiShip { get; set; } = new();
}
