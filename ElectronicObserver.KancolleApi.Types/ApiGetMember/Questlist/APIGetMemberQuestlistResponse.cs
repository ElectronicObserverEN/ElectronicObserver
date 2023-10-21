using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Questlist;

public class ApiGetMemberQuestlistResponse
{
	[JsonPropertyName("api_c_list")]
	public List<ApicList>? ApiCList { get; set; }

	[JsonPropertyName("api_completed_kind")]
	public int ApiCompletedKind { get; set; }

	[JsonPropertyName("api_count")]
	public int ApiCount { get; set; }

	[JsonPropertyName("api_disp_page")]
	public int ApiDispPage { get; set; }

	[JsonPropertyName("api_exec_count")]
	public int ApiExecCount { get; set; }

	[JsonPropertyName("api_exec_type")]
	public int ApiExecType { get; set; }

	/// <summary>
	/// Element type is <see cref="ApiListClass"/> or <see cref="int"/>.
	/// </summary>
	[JsonPropertyName("api_list")]
	public List<object> ApiList { get; set; } = new();

	[JsonPropertyName("api_page_count")]
	public int ApiPageCount { get; set; }
}
