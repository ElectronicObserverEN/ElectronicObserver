namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Payitemuse;

public class ApiReqMemberPayitemuseResponse
{
	[JsonPropertyName("api_caution_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCautionFlag { get; set; } = default!;

	[JsonPropertyName("api_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiFlag { get; set; } = default!;

	[JsonPropertyName("api_max_chara")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiMaxChara { get; set; } = default!;

	[JsonPropertyName("api_max_slotitem")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiMaxSlotitem { get; set; } = default!;
}
