namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstSlotitem
{
	[JsonPropertyName("api_atap")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiAtap { get; set; } = default!;

	[JsonPropertyName("api_bakk")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBakk { get; set; } = default!;

	[JsonPropertyName("api_baku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBaku { get; set; } = default!;

	[JsonPropertyName("api_broken")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiBroken { get; set; } = new();

	[JsonPropertyName("api_cost")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiCost { get; set; } = default!;

	[JsonPropertyName("api_distance")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiDistance { get; set; } = default!;

	[JsonPropertyName("api_houg")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiHoug { get; set; } = default!;

	[JsonPropertyName("api_houk")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiHouk { get; set; } = default!;

	[JsonPropertyName("api_houm")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiHoum { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiInfo { get; set; } = default!;

	[JsonPropertyName("api_leng")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLeng { get; set; } = default!;

	[JsonPropertyName("api_luck")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLuck { get; set; } = default!;

	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_raig")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRaig { get; set; } = default!;

	[JsonPropertyName("api_raik")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRaik { get; set; } = default!;

	[JsonPropertyName("api_raim")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRaim { get; set; } = default!;

	[JsonPropertyName("api_rare")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRare { get; set; } = default!;

	[JsonPropertyName("api_sakb")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSakb { get; set; } = default!;

	[JsonPropertyName("api_saku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSaku { get; set; } = default!;

	[JsonPropertyName("api_soku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSoku { get; set; } = default!;

	[JsonPropertyName("api_sortno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSortno { get; set; } = default!;

	[JsonPropertyName("api_souk")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSouk { get; set; } = default!;

	[JsonPropertyName("api_taik")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiTaik { get; set; } = default!;

	[JsonPropertyName("api_tais")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiTais { get; set; } = default!;

	[JsonPropertyName("api_tyku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiTyku { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiType { get; set; } = new();

	[JsonPropertyName("api_usebull")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiUsebull { get; set; } = default!;

	[JsonPropertyName("api_version")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiVersion { get; set; } = default!;
}
