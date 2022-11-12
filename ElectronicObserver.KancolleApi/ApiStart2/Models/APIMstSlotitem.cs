namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstSlotitem
{
	[System.Text.Json.Serialization.JsonPropertyName("api_atap")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiAtap { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_bakk")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiBakk { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_baku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiBaku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_broken")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiBroken { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_cost")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiCost { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_distance")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiDistance { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_houg")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiHoug { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_houk")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiHouk { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_houm")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiHoum { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiInfo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_leng")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLeng { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_luck")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLuck { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_raig")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiRaig { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_raik")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiRaik { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_raim")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiRaim { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_rare")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiRare { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_sakb")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSakb { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_saku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSaku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_soku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSoku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_sortno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSortno { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_souk")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSouk { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_taik")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiTaik { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_tais")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiTais { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_tyku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiTyku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiType { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_usebull")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiUsebull { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_version")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiVersion { get; set; } = default!;
}
