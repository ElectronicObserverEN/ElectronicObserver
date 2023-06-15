using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiKouku : IApiAirBattle
{
	[JsonPropertyName("api_plane_from")]
	[Required]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[JsonPropertyName("api_stage1")]
	[Required]
	public ApiStage1? ApiStage1 { get; set; } = new();

	[JsonPropertyName("api_stage2")]
	[Required]
	public ApiStage2? ApiStage2 { get; set; } = new();

	[JsonPropertyName("api_stage3")]
	[Required]
	public ApiStage3? ApiStage3 { get; set; } = new();

	[JsonPropertyName("api_stage3_combined")]
	[Required]
	public ApiStage3Combined? ApiStage3Combined { get; set; }
}
