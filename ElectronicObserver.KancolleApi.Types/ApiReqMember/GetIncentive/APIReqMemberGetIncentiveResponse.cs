using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetIncentive;

public class ApiReqMemberGetIncentiveResponse
{
	[JsonPropertyName("api_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCount { get; set; } = default!;

	[JsonPropertyName("api_item")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<ApiItem>? ApiItem { get; set; } = default!;
}
