namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyitem2;

public class ApiReqKousyouDestroyitem2Request
{
	[JsonPropertyName("api_slotitem_ids")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiSlotitemIds { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
