namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.PresetSelect;

public class ApiReqHenseiPresetSelectRequest
{
	[JsonPropertyName("api_deck_id")]
	public string ApiDeckId { get; set; } = "";

	[JsonPropertyName("api_preset_no")]
	public string ApiPresetNo { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
