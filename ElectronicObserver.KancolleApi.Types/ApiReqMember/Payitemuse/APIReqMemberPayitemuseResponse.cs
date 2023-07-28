namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Payitemuse;

public class ApiReqMemberPayitemuseResponse
{
	[JsonPropertyName("api_caution_flag")]
	public int ApiCautionFlag { get; set; }

	[JsonPropertyName("api_flag")]
	public int? ApiFlag { get; set; }

	[JsonPropertyName("api_max_chara")]
	public int? ApiMaxChara { get; set; }

	[JsonPropertyName("api_max_slotitem")]
	public int? ApiMaxSlotitem { get; set; }
}
