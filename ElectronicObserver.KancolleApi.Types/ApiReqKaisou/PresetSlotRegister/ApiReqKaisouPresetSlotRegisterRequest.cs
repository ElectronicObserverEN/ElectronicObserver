namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotRegister;

public class ApiReqKaisouPresetSlotRegisterRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("api_preset_id")]
	public string ApiPresetId { get; set; } = "";

	[JsonPropertyName("api_ship_id")]
	public string ApiShipId { get; set; } = "";
}
