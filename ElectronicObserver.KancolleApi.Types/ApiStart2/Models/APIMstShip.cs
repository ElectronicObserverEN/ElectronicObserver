namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstShip
{
	[JsonPropertyName("api_afterbull")]
	public int? ApiAfterbull { get; set; } = default!;

	[JsonPropertyName("api_afterfuel")]
	public int? ApiAfterfuel { get; set; } = default!;

	[JsonPropertyName("api_afterlv")]
	public int? ApiAfterlv { get; set; } = default!;

	[JsonPropertyName("api_aftershipid")]
	public string? ApiAftershipid { get; set; } = default!;

	[JsonPropertyName("api_backs")]
	public int? ApiBacks { get; set; } = default!;

	[JsonPropertyName("api_broken")]
	public List<int>? ApiBroken { get; set; } = default!;

	[JsonPropertyName("api_buildtime")]
	public int? ApiBuildtime { get; set; } = default!;

	[JsonPropertyName("api_bull_max")]
	public int? ApiBullMax { get; set; } = default!;

	[JsonPropertyName("api_ctype")]
	public int ApiCtype { get; set; } = default!;

	[JsonPropertyName("api_fuel_max")]
	public int? ApiFuelMax { get; set; } = default!;

	[JsonPropertyName("api_getmes")]
	public string? ApiGetmes { get; set; } = default!;

	[JsonPropertyName("api_houg")]
	public List<int>? ApiHoug { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_leng")]
	public int? ApiLeng { get; set; } = default!;

	[JsonPropertyName("api_luck")]
	public List<int>? ApiLuck { get; set; } = default!;

	[JsonPropertyName("api_maxeq")]
	public List<int>? ApiMaxeq { get; set; } = default!;

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_powup")]
	public List<int>? ApiPowup { get; set; } = default!;

	[JsonPropertyName("api_raig")]
	public List<int>? ApiRaig { get; set; } = default!;

	[JsonPropertyName("api_slot_num")]
	public int ApiSlotNum { get; set; } = default!;

	[JsonPropertyName("api_soku")]
	public int ApiSoku { get; set; } = default!;

	[JsonPropertyName("api_sort_id")]
	public int ApiSortId { get; set; } = default!;

	[JsonPropertyName("api_sortno")]
	public int? ApiSortno { get; set; } = default!;

	[JsonPropertyName("api_souk")]
	public List<int>? ApiSouk { get; set; } = default!;

	[JsonPropertyName("api_stype")]
	public int ApiStype { get; set; } = default!;

	[JsonPropertyName("api_taik")]
	public List<int>? ApiTaik { get; set; } = default!;

	[JsonPropertyName("api_tais")]
	public List<int>? ApiTais { get; set; } = default!;

	[JsonPropertyName("api_tyku")]
	public List<int>? ApiTyku { get; set; } = default!;

	[JsonPropertyName("api_voicef")]
	public int? ApiVoicef { get; set; } = default!;

	[JsonPropertyName("api_yomi")]
	public string ApiYomi { get; set; } = default!;
}
