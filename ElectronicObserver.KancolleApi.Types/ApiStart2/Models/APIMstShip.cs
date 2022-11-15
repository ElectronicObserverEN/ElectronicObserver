namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstShip
{
	[System.Text.Json.Serialization.JsonPropertyName("api_afterbull")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiAfterbull { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_afterfuel")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiAfterfuel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_afterlv")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiAfterlv { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_aftershipid")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiAftershipid { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_backs")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiBacks { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_broken")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiBroken { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_buildtime")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiBuildtime { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_bull_max")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiBullMax { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ctype")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCtype { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_fuel_max")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiFuelMax { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_getmes")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiGetmes { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_houg")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiHoug { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_leng")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiLeng { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_luck")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiLuck { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_maxeq")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiMaxeq { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_powup")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiPowup { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_raig")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiRaig { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_slot_num")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSlotNum { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_soku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSoku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_sort_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSortId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_sortno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiSortno { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_souk")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiSouk { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_stype")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiStype { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_taik")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiTaik { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_tais")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiTais { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_tyku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiTyku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_voicef")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiVoicef { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_yomi")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiYomi { get; set; } = default!;
}
