using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBaseAttack : IApiAirBattle
{
	[JsonPropertyName("api_base_id")]
	public int ApiBaseId { get; set; }

	[JsonPropertyName("api_plane_from")]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[JsonPropertyName("api_squadron_plane")]
	public List<ApiSquadronPlane> ApiSquadronPlane { get; set; } = new();

	[JsonPropertyName("api_stage1")]
	public ApiStage1? ApiStage1 { get; set; }

	[JsonPropertyName("api_stage2")]
	public ApiStage2? ApiStage2 { get; set; }

	[JsonPropertyName("api_stage3")]
	public ApiStage3? ApiStage3 { get; set; }

	[JsonPropertyName("api_stage3_combined")]
	public ApiStage3Combined? ApiStage3Combined { get; set; }

	[JsonPropertyName("api_stage_flag")]
	public List<int> ApiStageFlag { get; set; } = new();
}
