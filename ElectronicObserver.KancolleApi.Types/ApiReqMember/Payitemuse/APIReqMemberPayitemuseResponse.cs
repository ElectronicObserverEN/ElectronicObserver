namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Payitemuse;

public class ApiReqMemberPayitemuseResponse
{
	[JsonPropertyName("api_caution_flag")]
	public int ApiCautionFlag { get; set; } = default!;

	[JsonPropertyName("api_flag")]
	public int? ApiFlag { get; set; } = default!;

	[JsonPropertyName("api_max_chara")]
	public int? ApiMaxChara { get; set; } = default!;

	[JsonPropertyName("api_max_slotitem")]
	public int? ApiMaxSlotitem { get; set; } = default!;
}
