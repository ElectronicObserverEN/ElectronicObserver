namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotSelect;

public class ApiReqKaisouPresetSlotSelectRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("api_preset_id")]
	public string ApiPresetId { get; set; } = "";

	[JsonPropertyName("api_ship_id")]
	public string ApiShipId { get; set; } = "";

	[JsonPropertyName("api_equip_mode")]
	public string ApiEquipMode { get; set; } = "";
}
