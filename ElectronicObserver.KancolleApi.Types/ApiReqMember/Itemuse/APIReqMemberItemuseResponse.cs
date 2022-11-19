using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Itemuse;

public class ApiReqMemberItemuseResponse
{
	[JsonPropertyName("api_caution_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCautionFlag { get; set; } = default!;

	[JsonPropertyName("api_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFlag { get; set; } = default!;

	[JsonPropertyName("api_getitem")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiGetitem?> ApiGetitem { get; set; } = new();

	[JsonPropertyName("api_material")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiMaterial { get; set; } = default!;
}
