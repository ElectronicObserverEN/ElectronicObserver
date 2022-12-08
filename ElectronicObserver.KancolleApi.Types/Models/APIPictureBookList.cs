namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPictureBookList
{
	[JsonPropertyName("api_baku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiBaku { get; set; } = default!;

	[JsonPropertyName("api_cnum")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiCnum { get; set; } = default!;

	[JsonPropertyName("api_ctype")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiCtype { get; set; } = default!;

	[JsonPropertyName("api_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiFlag { get; set; } = default!;

	[JsonPropertyName("api_houg")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiHoug { get; set; } = default!;

	[JsonPropertyName("api_houk")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiHouk { get; set; } = default!;

	[JsonPropertyName("api_houm")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiHoum { get; set; } = default!;

	[JsonPropertyName("api_index_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiIndexNo { get; set; } = default!;

	[JsonPropertyName("api_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiInfo { get; set; } = default!;

	[JsonPropertyName("api_kaih")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiKaih { get; set; } = default!;

	[JsonPropertyName("api_leng")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLeng { get; set; } = default!;

	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_q_voice_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<ApiqVoiceInfo>? ApiQVoiceInfo { get; set; } = default!;

	[JsonPropertyName("api_raig")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRaig { get; set; } = default!;

	[JsonPropertyName("api_saku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiSaku { get; set; } = default!;

	[JsonPropertyName("api_sinfo")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiSinfo { get; set; } = default!;

	[JsonPropertyName("api_soku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiSoku { get; set; } = default!;

	[JsonPropertyName("api_souk")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSouk { get; set; } = default!;

	[JsonPropertyName("api_state")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiState { get; set; } = new();

	[JsonPropertyName("api_stype")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiStype { get; set; } = default!;

	[JsonPropertyName("api_table_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiTableId { get; set; } = new();

	[JsonPropertyName("api_taik")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiTaik { get; set; } = default!;

	[JsonPropertyName("api_tais")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiTais { get; set; } = default!;

	[JsonPropertyName("api_tyku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiTyku { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiType { get; set; } = default!;

	[JsonPropertyName("api_yomi")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiYomi { get; set; } = default!;
}
