using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetIncentive;

public class ApiReqMemberGetIncentiveResponse
{
	[JsonPropertyName("api_count")]
	public int ApiCount { get; set; } = default!;

	[JsonPropertyName("api_item")]
	public List<ApiItem>? ApiItem { get; set; } = default!;
}
