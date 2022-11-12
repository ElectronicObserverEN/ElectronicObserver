using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Questlist;

public class ApiGetMemberQuestlistResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_c_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<ApicList>? ApiCList { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_completed_kind")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCompletedKind { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_disp_page")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiDispPage { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_exec_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiExecCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_exec_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiExecType { get; set; } = default!;

	/// <summary>
	/// Element type is <see cref="ApiListClass"/> or <see cref="int"/>.
	/// </summary>
	[System.Text.Json.Serialization.JsonPropertyName("api_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<object> ApiList { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_page_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiPageCount { get; set; } = default!;
}
