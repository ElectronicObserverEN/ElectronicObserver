using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Itemuse;

public class ApiReqMemberItemuseResponse
{
	[JsonPropertyName("api_caution_flag")]
	public int ApiCautionFlag { get; set; }

	[JsonPropertyName("api_flag")]
	public int ApiFlag { get; set; }

	[JsonPropertyName("api_getitem")]
	public List<ApiGetitem?> ApiGetitem { get; set; } = new();

	[JsonPropertyName("api_material")]
	public List<int>? ApiMaterial { get; set; }
}
