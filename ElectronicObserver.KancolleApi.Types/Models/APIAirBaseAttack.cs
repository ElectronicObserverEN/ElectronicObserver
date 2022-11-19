using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBaseAttack
{
	[JsonPropertyName("api_base_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBaseId { get; set; } = default!;

	[JsonPropertyName("api_plane_from")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[JsonPropertyName("api_squadron_plane")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiSquadronPlane> ApiSquadronPlane { get; set; } = new();

	[JsonPropertyName("api_stage1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiStage1 ApiStage1 { get; set; } = new();

	[JsonPropertyName("api_stage2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiStage? ApiStage2 { get; set; } = default!;

	[JsonPropertyName("api_stage3")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiStage3 ApiStage3 { get; set; } = default!;

	[JsonPropertyName("api_stage_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiStageFlag { get; set; } = new();
}
