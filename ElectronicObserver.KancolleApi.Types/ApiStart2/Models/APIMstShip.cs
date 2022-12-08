namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstShip
{
	[JsonPropertyName("api_afterbull")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiAfterbull { get; set; } = default!;

	[JsonPropertyName("api_afterfuel")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiAfterfuel { get; set; } = default!;

	[JsonPropertyName("api_afterlv")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiAfterlv { get; set; } = default!;

	[JsonPropertyName("api_aftershipid")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiAftershipid { get; set; } = default!;

	[JsonPropertyName("api_backs")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiBacks { get; set; } = default!;

	[JsonPropertyName("api_broken")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiBroken { get; set; } = default!;

	[JsonPropertyName("api_buildtime")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiBuildtime { get; set; } = default!;

	[JsonPropertyName("api_bull_max")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiBullMax { get; set; } = default!;

	[JsonPropertyName("api_ctype")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCtype { get; set; } = default!;

	[JsonPropertyName("api_fuel_max")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiFuelMax { get; set; } = default!;

	[JsonPropertyName("api_getmes")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiGetmes { get; set; } = default!;

	[JsonPropertyName("api_houg")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiHoug { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_leng")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiLeng { get; set; } = default!;

	[JsonPropertyName("api_luck")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiLuck { get; set; } = default!;

	[JsonPropertyName("api_maxeq")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiMaxeq { get; set; } = default!;

	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_powup")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiPowup { get; set; } = default!;

	[JsonPropertyName("api_raig")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiRaig { get; set; } = default!;

	[JsonPropertyName("api_slot_num")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSlotNum { get; set; } = default!;

	[JsonPropertyName("api_soku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSoku { get; set; } = default!;

	[JsonPropertyName("api_sort_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSortId { get; set; } = default!;

	[JsonPropertyName("api_sortno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiSortno { get; set; } = default!;

	[JsonPropertyName("api_souk")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiSouk { get; set; } = default!;

	[JsonPropertyName("api_stype")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiStype { get; set; } = default!;

	[JsonPropertyName("api_taik")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiTaik { get; set; } = default!;

	[JsonPropertyName("api_tais")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiTais { get; set; } = default!;

	[JsonPropertyName("api_tyku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiTyku { get; set; } = default!;

	[JsonPropertyName("api_voicef")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiVoicef { get; set; } = default!;

	[JsonPropertyName("api_yomi")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiYomi { get; set; } = default!;
}
