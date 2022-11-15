namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBaseInjection
{
	[System.Text.Json.Serialization.JsonPropertyName("api_air_base_data")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiAirBaseDatumElement> ApiAirBaseData { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_plane_from")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<object?> ApiPlaneFrom { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_stage1")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiStage ApiStage1 { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_stage2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiStage ApiStage2 { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_stage3")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiAirBaseAttackApiStage3 ApiStage3 { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_stage3_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiAirBaseAttackApiStage3 ApiStage3Combined { get; set; } = new();
}
