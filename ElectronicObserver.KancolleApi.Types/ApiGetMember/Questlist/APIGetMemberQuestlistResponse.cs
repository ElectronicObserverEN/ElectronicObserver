using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Questlist;

public class ApiGetMemberQuestlistResponse
{
	[JsonPropertyName("api_c_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<ApicList>? ApiCList { get; set; } = default!;

	[JsonPropertyName("api_completed_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCompletedKind { get; set; } = default!;

	[JsonPropertyName("api_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCount { get; set; } = default!;

	[JsonPropertyName("api_disp_page")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDispPage { get; set; } = default!;

	[JsonPropertyName("api_exec_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiExecCount { get; set; } = default!;

	[JsonPropertyName("api_exec_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiExecType { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="ApiListClass"/> or <see cref="int"/>.
	/// </summary>
	[JsonPropertyName("api_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<object> ApiList { get; set; } = new();

	[JsonPropertyName("api_page_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPageCount { get; set; } = default!;
}
