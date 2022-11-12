namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBaseAttack
{
	[System.Text.Json.Serialization.JsonPropertyName("api_base_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiBaseId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_plane_from")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_squadron_plane")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiSquadronPlane> ApiSquadronPlane { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_stage1")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiStage1 ApiStage1 { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_stage2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiStage? ApiStage2 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_stage3")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiInjectionKoukuApiStage3 ApiStage3 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_stage_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiStageFlag { get; set; } = new();
}
