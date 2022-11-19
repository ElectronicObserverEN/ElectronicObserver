using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetEventSelectedReward;

public class ApiReqMemberGetEventSelectedRewardResponse
{
	[JsonPropertyName("api_get_item_list")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiGetItemList> ApiGetItemList { get; set; } = new();
}
