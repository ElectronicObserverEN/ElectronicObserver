namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Payitemuse;

public class ApiReqMemberPayitemuseResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_caution_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCautionFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_max_chara")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiMaxChara { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_max_slotitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiMaxSlotitem { get; set; } = default!;
}
