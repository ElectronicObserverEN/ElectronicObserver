namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.CanPresetSlotSelect;

public class APIReqKaisouCanPresetSlotSelectResponse
{
	[JsonPropertyName("api_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiFlag { get; set; } = default!;
}
