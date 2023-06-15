using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiKouku
{
	[JsonPropertyName("api_plane_from")]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[JsonPropertyName("api_stage1")]
	public ApiStage1? ApiStage1 { get; set; } = new();

	[JsonPropertyName("api_stage2")]
	public ApiStage2? ApiStage2 { get; set; }

	[JsonPropertyName("api_stage3")]
	public ApiStage3? ApiStage3 { get; set; }
}
