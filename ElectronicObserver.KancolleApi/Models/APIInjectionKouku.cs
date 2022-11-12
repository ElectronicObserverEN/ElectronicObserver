namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiInjectionKouku
{
	[System.Text.Json.Serialization.JsonPropertyName("api_plane_from")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

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
	public ApiInjectionKoukuApiStage3 ApiStage3 { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_stage3_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiStage3Combined ApiStage3Combined { get; set; } = new();
}
