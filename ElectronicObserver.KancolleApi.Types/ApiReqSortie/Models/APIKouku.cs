namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiKouku
{
	[JsonPropertyName("api_plane_from")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[JsonPropertyName("api_stage1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiStage1 ApiStage1 { get; set; } = new();

	[JsonPropertyName("api_stage2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiStage2? ApiStage2 { get; set; } = default!;

	[JsonPropertyName("api_stage3")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiStage3? ApiStage3 { get; set; } = default!;
}
