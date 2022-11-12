using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Practice;

public class ApiGetMemberPracticeResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_create_kind")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCreateKind { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_entry_limit")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiEntryLimit { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiPracticeList> ApiList { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_selected_kind")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSelectedKind { get; set; } = default!;

}
