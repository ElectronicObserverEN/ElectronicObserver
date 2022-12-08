namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Questlist;

public class ApiGetMemberQuestlistRequest
{
	[JsonPropertyName("api_page_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiPageNo { get; set; } = default!;

	[JsonPropertyName("api_tab_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiTabId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
