using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Mission;

public class ApiGetMemberMissionResponse
{
	[JsonPropertyName("api_limit_time")]
	[Required]
	public List<int> ApiLimitTime { get; set; } = new();

	[JsonPropertyName("api_list_items")]
	[Required]
	public List<ApiListItems> ApiListItems { get; set; } = new();
}
