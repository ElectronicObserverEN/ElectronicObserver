namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotRegister;

public class ApiReqKaisouPresetSlotRegisterRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_preset_id")]
	public string ApiPresetId { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	public string ApiShipId { get; set; } = default!;
}
