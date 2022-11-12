namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMapinfo
{
	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_infotext")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiInfotext { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_item")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiItem { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_maparea_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMapareaId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_max_maphp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int? ApiMaxMaphp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_opetext")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiOpetext { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_required_defeat_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int? ApiRequiredDefeatCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_sally_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiSallyFlag { get; set; } = new();
}
