namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPictureBookList
{
	[JsonPropertyName("api_baku")]
	public int? ApiBaku { get; set; }

	[JsonPropertyName("api_cnum")]
	public int? ApiCnum { get; set; }

	[JsonPropertyName("api_ctype")]
	public int? ApiCtype { get; set; }

	[JsonPropertyName("api_flag")]
	public List<int>? ApiFlag { get; set; }

	[JsonPropertyName("api_houg")]
	public int ApiHoug { get; set; }

	[JsonPropertyName("api_houk")]
	public int? ApiHouk { get; set; }

	[JsonPropertyName("api_houm")]
	public int? ApiHoum { get; set; }

	[JsonPropertyName("api_index_no")]
	public int ApiIndexNo { get; set; }

	[JsonPropertyName("api_info")]
	public string? ApiInfo { get; set; }

	[JsonPropertyName("api_kaih")]
	public int? ApiKaih { get; set; }

	[JsonPropertyName("api_leng")]
	public int ApiLeng { get; set; }

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_q_voice_info")]
	public List<ApiqVoiceInfo>? ApiQVoiceInfo { get; set; }

	[JsonPropertyName("api_raig")]
	public int ApiRaig { get; set; }

	[JsonPropertyName("api_saku")]
	public int? ApiSaku { get; set; }

	[JsonPropertyName("api_sinfo")]
	public string? ApiSinfo { get; set; }

	[JsonPropertyName("api_soku")]
	public int? ApiSoku { get; set; }

	[JsonPropertyName("api_souk")]
	public int ApiSouk { get; set; }

	[JsonPropertyName("api_state")]
	public List<List<int>> ApiState { get; set; } = new();

	[JsonPropertyName("api_stype")]
	public int? ApiStype { get; set; }

	[JsonPropertyName("api_table_id")]
	public List<int> ApiTableId { get; set; } = new();

	[JsonPropertyName("api_taik")]
	public int? ApiTaik { get; set; }

	[JsonPropertyName("api_tais")]
	public int ApiTais { get; set; }

	[JsonPropertyName("api_tyku")]
	public int ApiTyku { get; set; }

	[JsonPropertyName("api_type")]
	public List<int>? ApiType { get; set; }

	[JsonPropertyName("api_yomi")]
	public string? ApiYomi { get; set; }
}
