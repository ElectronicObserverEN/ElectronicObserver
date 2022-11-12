namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiKouku
{
	[System.Text.Json.Serialization.JsonPropertyName("api_plane_from")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_stage1")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiStage1 ApiStage1 { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_stage2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiStage2? ApiStage2 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_stage3")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiStage3? ApiStage3 { get; set; } = default!;
}
