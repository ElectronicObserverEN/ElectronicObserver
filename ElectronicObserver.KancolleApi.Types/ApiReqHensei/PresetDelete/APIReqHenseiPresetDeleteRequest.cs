namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.PresetDelete;

public class ApiReqHenseiPresetDeleteRequest
{
	[JsonPropertyName("api_preset_no")]
	public string ApiPresetNo { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
