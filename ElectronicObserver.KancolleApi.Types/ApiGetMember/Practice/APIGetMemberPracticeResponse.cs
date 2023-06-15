using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Practice;

public class ApiGetMemberPracticeResponse
{
	[JsonPropertyName("api_create_kind")]
	public int ApiCreateKind { get; set; }

	[JsonPropertyName("api_entry_limit")]
	public int? ApiEntryLimit { get; set; }

	[JsonPropertyName("api_list")]
	public List<ApiPracticeList> ApiList { get; set; } = new();

	[JsonPropertyName("api_selected_kind")]
	public int ApiSelectedKind { get; set; }

}
