namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBaseInjection
{
	[JsonPropertyName("api_air_base_data")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiAirBaseDatumElement> ApiAirBaseData { get; set; } = new();

	[JsonPropertyName("api_plane_from")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<object?> ApiPlaneFrom { get; set; } = new();

	[JsonPropertyName("api_stage1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiStage ApiStage1 { get; set; } = new();

	[JsonPropertyName("api_stage2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiStage ApiStage2 { get; set; } = new();

	[JsonPropertyName("api_stage3")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiAirBaseAttackApiStage3 ApiStage3 { get; set; } = new();

	[JsonPropertyName("api_stage3_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiAirBaseAttackApiStage3 ApiStage3Combined { get; set; } = new();
}
