namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.CanPresetSlotSelect;

public class APIReqKaisouCanPresetSlotSelectResponse
{
	[JsonPropertyName("api_flag")]
	[Required]
	public int ApiFlag { get; set; } = default!;
}
