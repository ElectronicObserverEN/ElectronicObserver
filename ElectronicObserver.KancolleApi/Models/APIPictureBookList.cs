namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPictureBookList
{
	[System.Text.Json.Serialization.JsonPropertyName("api_baku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiBaku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_cnum")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiCnum { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ctype")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiCtype { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_houg")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiHoug { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_houk")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiHouk { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_houm")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiHoum { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_index_no")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiIndexNo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiInfo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_kaih")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiKaih { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_leng")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLeng { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_q_voice_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<ApiqVoiceInfo>? ApiQVoiceInfo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_raig")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiRaig { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_saku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiSaku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_sinfo")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiSinfo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_soku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiSoku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_souk")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSouk { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_state")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiState> ApiState { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_stype")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiStype { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_table_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiTableId { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_taik")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiTaik { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_tais")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiTais { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_tyku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiTyku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiType { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_yomi")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiYomi { get; set; } = default!;
}
