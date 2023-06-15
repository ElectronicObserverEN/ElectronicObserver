namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Questlist;

public class ApiGetMemberQuestlistRequest
{
	[JsonPropertyName("api_page_no")]
	[Required(AllowEmptyStrings = true)]
	public string ApiPageNo { get; set; } = default!;

	[JsonPropertyName("api_tab_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiTabId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
