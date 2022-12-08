namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.PresetSelect;

public class ApiReqHenseiPresetSelectRequest
{
	[JsonPropertyName("api_deck_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_preset_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiPresetNo { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
