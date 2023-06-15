namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotSelect;

public class ApiReqKaisouPresetSlotSelectRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_preset_id")]
	public string ApiPresetId { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	public string ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_equip_mode")]
	public string ApiEquipMode { get; set; } = default!;
}
