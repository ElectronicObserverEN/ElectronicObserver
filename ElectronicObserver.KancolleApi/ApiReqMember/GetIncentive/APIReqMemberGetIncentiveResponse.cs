using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetIncentive;

public class ApiReqMemberGetIncentiveResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<ApiItem>? ApiItem { get; set; } = default!;
}
