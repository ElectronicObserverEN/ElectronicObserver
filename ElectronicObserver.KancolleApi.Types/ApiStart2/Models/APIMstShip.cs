namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstShip
{
	[JsonPropertyName("api_afterbull")]
	public int? ApiAfterbull { get; set; }

	[JsonPropertyName("api_afterfuel")]
	public int? ApiAfterfuel { get; set; }

	[JsonPropertyName("api_afterlv")]
	public int? ApiAfterlv { get; set; }

	[JsonPropertyName("api_aftershipid")]
	public string? ApiAftershipid { get; set; }

	[JsonPropertyName("api_backs")]
	public int? ApiBacks { get; set; }

	[JsonPropertyName("api_broken")]
	public List<int>? ApiBroken { get; set; }

	[JsonPropertyName("api_buildtime")]
	public int? ApiBuildtime { get; set; }

	[JsonPropertyName("api_bull_max")]
	public int? ApiBullMax { get; set; }

	[JsonPropertyName("api_ctype")]
	public int ApiCtype { get; set; }

	[JsonPropertyName("api_fuel_max")]
	public int? ApiFuelMax { get; set; }

	[JsonPropertyName("api_getmes")]
	public string? ApiGetmes { get; set; }

	[JsonPropertyName("api_houg")]
	public List<int>? ApiHoug { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_leng")]
	public int? ApiLeng { get; set; }

	[JsonPropertyName("api_luck")]
	public List<int>? ApiLuck { get; set; }

	[JsonPropertyName("api_maxeq")]
	public List<int>? ApiMaxeq { get; set; }

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_powup")]
	public List<int>? ApiPowup { get; set; }

	[JsonPropertyName("api_raig")]
	public List<int>? ApiRaig { get; set; }

	[JsonPropertyName("api_slot_num")]
	public int ApiSlotNum { get; set; }

	[JsonPropertyName("api_soku")]
	public int ApiSoku { get; set; }

	[JsonPropertyName("api_sort_id")]
	public int ApiSortId { get; set; }

	[JsonPropertyName("api_sortno")]
	public int? ApiSortno { get; set; }

	[JsonPropertyName("api_souk")]
	public List<int>? ApiSouk { get; set; }

	[JsonPropertyName("api_stype")]
	public int ApiStype { get; set; }

	[JsonPropertyName("api_taik")]
	public List<int>? ApiTaik { get; set; }

	[JsonPropertyName("api_tais")]
	public List<int>? ApiTais { get; set; }

	[JsonPropertyName("api_tyku")]
	public List<int>? ApiTyku { get; set; }

	[JsonPropertyName("api_voicef")]
	public int? ApiVoicef { get; set; }

	[JsonPropertyName("api_yomi")]
	public string ApiYomi { get; set; } = "";
}
