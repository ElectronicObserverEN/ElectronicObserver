using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetEventSelectedReward;

public class ApiReqMemberGetEventSelectedRewardResponse
{
	[JsonPropertyName("api_get_item_list")]
	public List<ApiGetItemList> ApiGetItemList { get; set; } = new();
}
