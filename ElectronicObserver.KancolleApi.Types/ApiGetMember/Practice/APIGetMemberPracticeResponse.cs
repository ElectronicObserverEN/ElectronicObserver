using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Practice;

public class ApiGetMemberPracticeResponse
{
	[JsonPropertyName("api_create_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCreateKind { get; set; } = default!;

	[JsonPropertyName("api_entry_limit")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiEntryLimit { get; set; } = default!;

	[JsonPropertyName("api_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiPracticeList> ApiList { get; set; } = new();

	[JsonPropertyName("api_selected_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSelectedKind { get; set; } = default!;

}
