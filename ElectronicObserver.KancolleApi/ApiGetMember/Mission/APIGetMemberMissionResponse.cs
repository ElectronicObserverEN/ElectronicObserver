using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Mission;

public class ApiGetMemberMissionResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_limit_time")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiLimitTime { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_list_items")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiListItems> ApiListItems { get; set; } = new();
}
