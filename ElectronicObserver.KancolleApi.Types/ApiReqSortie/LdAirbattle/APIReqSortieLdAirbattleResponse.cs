using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdAirbattle;

public class ApiReqSortieLdAirbattleResponse
{
	[JsonPropertyName("api_air_base_attack")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<ApiAirBaseAttack>? ApiAirBaseAttack { get; set; } = default!;

	[JsonPropertyName("api_deck_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_eParam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiEParam { get; set; } = new();

	[JsonPropertyName("api_eSlot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiESlot { get; set; } = new();

	[JsonPropertyName("api_e_maxhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEMaxhps { get; set; } = new();

	[JsonPropertyName("api_e_nowhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiENowhps { get; set; } = new();

	[JsonPropertyName("api_fParam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiFParam { get; set; } = new();

	[JsonPropertyName("api_f_maxhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[JsonPropertyName("api_f_nowhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFNowhps { get; set; } = new();

	[JsonPropertyName("api_formation")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFormation { get; set; } = new();

	[JsonPropertyName("api_kouku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiKouku ApiKouku { get; set; } = new();

	[JsonPropertyName("api_midnight_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMidnightFlag { get; set; } = default!;

	[JsonPropertyName("api_search")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiSearch { get; set; } = new();

	[JsonPropertyName("api_ship_ke")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipKe { get; set; } = new();

	[JsonPropertyName("api_ship_lv")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipLv { get; set; } = new();

	[JsonPropertyName("api_stage_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiStageFlag { get; set; } = new();
}
