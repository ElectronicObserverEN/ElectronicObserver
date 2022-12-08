namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMapinfo
{
	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_infotext")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiInfotext { get; set; } = default!;

	[JsonPropertyName("api_item")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiItem { get; set; } = new();

	[JsonPropertyName("api_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_maparea_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMapareaId { get; set; } = default!;

	[JsonPropertyName("api_max_maphp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiMaxMaphp { get; set; } = default!;

	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_opetext")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiOpetext { get; set; } = default!;

	[JsonPropertyName("api_required_defeat_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiRequiredDefeatCount { get; set; } = default!;

	[JsonPropertyName("api_sally_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiSallyFlag { get; set; } = new();
}
