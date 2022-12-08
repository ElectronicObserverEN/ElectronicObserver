namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.RemodelSlot;

public class ApiReqKousyouRemodelSlotRequest
{
	[JsonPropertyName("api_certain_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiCertainFlag { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiId { get; set; } = default!;

	[JsonPropertyName("api_slot_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiSlotId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
