namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.PresetSlotUpdateExslotFlag;

public class ApiReqKaisouPresetSlotUpdateExslotFlagRequest
{
	[JsonPropertyName("api_token")]
	public required string ApiToken { get; set; }

	[JsonPropertyName("api_verno")]
	public required string ApiVerno { get; set; }

	[JsonPropertyName("api_preset_id")]
	public required string ApiPresetId { get; set; }
}
