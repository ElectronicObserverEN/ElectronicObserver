using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Itemuse;

public class ApiReqMemberItemuseResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_caution_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCautionFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_getitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiGetitem?> ApiGetitem { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_material")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiMaterial { get; set; } = default!;
}
