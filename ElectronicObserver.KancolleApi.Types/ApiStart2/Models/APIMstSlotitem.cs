namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstSlotitem
{
	[JsonPropertyName("api_atap")]
	public int ApiAtap { get; set; }

	[JsonPropertyName("api_bakk")]
	public int ApiBakk { get; set; }

	[JsonPropertyName("api_baku")]
	public int ApiBaku { get; set; }

	[JsonPropertyName("api_broken")]
	public List<int> ApiBroken { get; set; } = new();

	[JsonPropertyName("api_cost")]
	public int? ApiCost { get; set; }

	[JsonPropertyName("api_distance")]
	public int? ApiDistance { get; set; }

	[JsonPropertyName("api_houg")]
	public int ApiHoug { get; set; }

	[JsonPropertyName("api_houk")]
	public int ApiHouk { get; set; }

	[JsonPropertyName("api_houm")]
	public int ApiHoum { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_info")]
	public string? ApiInfo { get; set; }

	[JsonPropertyName("api_leng")]
	public int ApiLeng { get; set; }

	[JsonPropertyName("api_luck")]
	public int ApiLuck { get; set; }

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_raig")]
	public int ApiRaig { get; set; }

	[JsonPropertyName("api_raik")]
	public int ApiRaik { get; set; }

	[JsonPropertyName("api_raim")]
	public int ApiRaim { get; set; }

	[JsonPropertyName("api_rare")]
	public int ApiRare { get; set; }

	[JsonPropertyName("api_sakb")]
	public int ApiSakb { get; set; }

	[JsonPropertyName("api_saku")]
	public int ApiSaku { get; set; }

	[JsonPropertyName("api_soku")]
	public int ApiSoku { get; set; }

	[JsonPropertyName("api_sortno")]
	public int ApiSortno { get; set; }

	[JsonPropertyName("api_souk")]
	public int ApiSouk { get; set; }

	[JsonPropertyName("api_taik")]
	public int ApiTaik { get; set; }

	[JsonPropertyName("api_tais")]
	public int ApiTais { get; set; }

	[JsonPropertyName("api_tyku")]
	public int ApiTyku { get; set; }

	[JsonPropertyName("api_type")]
	public List<int> ApiType { get; set; } = new();

	[JsonPropertyName("api_usebull")]
	public string ApiUsebull { get; set; } = "";

	[JsonPropertyName("api_version")]
	public int? ApiVersion { get; set; }
}
