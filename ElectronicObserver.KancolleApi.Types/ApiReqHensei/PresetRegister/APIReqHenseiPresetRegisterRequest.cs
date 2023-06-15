namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.PresetRegister;

public class ApiReqHenseiPresetRegisterRequest
{
	[JsonPropertyName("api_deck_id")]
	public string ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_name_id")]
	public string ApiNameId { get; set; } = default!;

	[JsonPropertyName("api_preset_no")]
	public string ApiPresetNo { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
