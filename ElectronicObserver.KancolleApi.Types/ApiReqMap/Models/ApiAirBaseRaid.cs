using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiAirBaseRaid
{
	[JsonPropertyName("api_plane_from")]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	/// <summary>
	/// 参加機リスト　防空状態の基地が存在しない場合 null
	/// </summary>
	[JsonPropertyName("api_map_squadron_plane")]
	public Dictionary<string, List<ApiSquadronPlane>>? ApiMapSquadronPlane { get; set; }

	[JsonPropertyName("api_stage1")]
	public ApiStage1? ApiStage1 { get; set; } = new();

	[JsonPropertyName("api_stage2")]
	public ApiStage2? ApiStage2 { get; set; }

	[JsonPropertyName("api_stage3")]
	public ApiStage3? ApiStage3 { get; set; }

	[JsonPropertyName("api_stage_flag")]
	public List<int> ApiStageFlag { get; set; } = new();
}
