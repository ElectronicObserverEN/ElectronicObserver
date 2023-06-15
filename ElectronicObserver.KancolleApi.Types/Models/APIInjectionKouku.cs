using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiInjectionKouku : IApiJetAirBattle
{
	[JsonPropertyName("api_plane_from")]
	[Required]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[JsonPropertyName("api_stage1")]
	[Required]
	public ApiStage1And2Jet? ApiStage1 { get; set; } = new();

	[JsonPropertyName("api_stage2")]
	[Required]
	public ApiStage1And2Jet? ApiStage2 { get; set; } = new();

	[JsonPropertyName("api_stage3")]
	[Required]
	public ApiStage3Jet ApiStage3 { get; set; } = new();

	[JsonPropertyName("api_stage3_combined")]
	[Required]
	public ApiStage3JetCombined ApiStage3Combined { get; set; } = new();
}
