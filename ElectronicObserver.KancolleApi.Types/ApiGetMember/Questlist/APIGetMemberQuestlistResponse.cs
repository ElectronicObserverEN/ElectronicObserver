using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Questlist;

public class ApiGetMemberQuestlistResponse
{
	[JsonPropertyName("api_c_list")]
	public List<ApicList>? ApiCList { get; set; } = default!;

	[JsonPropertyName("api_completed_kind")]
	public int ApiCompletedKind { get; set; } = default!;

	[JsonPropertyName("api_count")]
	public int ApiCount { get; set; } = default!;

	[JsonPropertyName("api_disp_page")]
	public int ApiDispPage { get; set; } = default!;

	[JsonPropertyName("api_exec_count")]
	public int ApiExecCount { get; set; } = default!;

	[JsonPropertyName("api_exec_type")]
	public int ApiExecType { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="ApiListClass"/> or <see cref="int"/>.
	/// </summary>
	[JsonPropertyName("api_list")]
	[Required]
	public List<object> ApiList { get; set; } = new();

	[JsonPropertyName("api_page_count")]
	public int ApiPageCount { get; set; } = default!;
}
