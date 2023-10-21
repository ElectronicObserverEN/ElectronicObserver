using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBaseInjection : IApiJetAirBattle
{
	[JsonPropertyName("api_air_base_data")]
	public List<ApiSquadronPlane> ApiAirBaseData { get; set; } = new();

	[JsonPropertyName("api_plane_from")]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[JsonPropertyName("api_stage1")]
	public ApiStage1And2Jet? ApiStage1 { get; set; }

	[JsonPropertyName("api_stage2")]
	public ApiStage1And2Jet? ApiStage2 { get; set; }

	[JsonPropertyName("api_stage3")]
	public ApiStage3Jet? ApiStage3 { get; set; }

	[JsonPropertyName("api_stage3_combined")]
	public ApiStage3JetCombined? ApiStage3Combined { get; set; }
}
