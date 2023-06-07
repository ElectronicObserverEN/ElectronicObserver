using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBaseInjection : IApiJetAirBattle
{
	[JsonPropertyName("api_air_base_data")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiSquadronPlane> ApiAirBaseData { get; set; } = new();

	[JsonPropertyName("api_plane_from")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[JsonPropertyName("api_stage1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiStage1And2Jet ApiStage1 { get; set; } = new();

	[JsonPropertyName("api_stage2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiStage1And2Jet ApiStage2 { get; set; } = new();

	[JsonPropertyName("api_stage3")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiStage3Jet ApiStage3 { get; set; } = new();

	[JsonPropertyName("api_stage3_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiStage3JetCombined ApiStage3Combined { get; set; } = new();
}
