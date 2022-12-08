using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Mission;

public class ApiGetMemberMissionResponse
{
	[JsonPropertyName("api_limit_time")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiLimitTime { get; set; } = new();

	[JsonPropertyName("api_list_items")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiListItems> ApiListItems { get; set; } = new();
}
