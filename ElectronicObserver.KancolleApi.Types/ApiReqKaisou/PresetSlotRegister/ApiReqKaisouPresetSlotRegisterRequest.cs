namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotRegister;

public class ApiReqKaisouPresetSlotRegisterRequest
{
	[JsonPropertyName("api_token")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_preset_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiPresetId { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipId { get; set; } = default!;
}
