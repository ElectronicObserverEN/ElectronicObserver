namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Questlist;

public class ApiGetMemberQuestlistRequest
{
	[JsonPropertyName("api_page_no")]
	public string ApiPageNo { get; set; } = "";

	[JsonPropertyName("api_tab_id")]
	public string ApiTabId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
