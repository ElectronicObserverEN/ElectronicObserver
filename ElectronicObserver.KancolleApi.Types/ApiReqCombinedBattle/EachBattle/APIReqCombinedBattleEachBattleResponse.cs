using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;

public class ApiReqCombinedBattleEachBattleResponse
{
	[JsonPropertyName("api_air_base_attack")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiAirBaseAttack> ApiAirBaseAttack { get; set; } = new();

	[JsonPropertyName("api_air_base_injection")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiAirBaseInjection? ApiAirBaseInjection { get; set; } = default!;

	[JsonPropertyName("api_deck_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_eParam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiEParam { get; set; } = new();

	[JsonPropertyName("api_eParam_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiEParamCombined { get; set; } = new();

	[JsonPropertyName("api_eSlot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiESlot { get; set; } = new();

	[JsonPropertyName("api_eSlot_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiESlotCombined { get; set; } = new();

	[JsonPropertyName("api_e_maxhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEMaxhps { get; set; } = new();

	[JsonPropertyName("api_e_maxhps_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEMaxhpsCombined { get; set; } = new();

	[JsonPropertyName("api_e_nowhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiENowhps { get; set; } = new();

	[JsonPropertyName("api_e_nowhps_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiENowhpsCombined { get; set; } = new();

	[JsonPropertyName("api_escape_idx")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiEscapeIdx { get; set; } = default!;

	[JsonPropertyName("api_escape_idx_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiEscapeIdxCombined { get; set; } = default!;

	[JsonPropertyName("api_fParam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiFParam { get; set; } = new();

	[JsonPropertyName("api_fParam_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiFParamCombined { get; set; } = new();

	[JsonPropertyName("api_f_maxhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[JsonPropertyName("api_f_maxhps_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFMaxhpsCombined { get; set; } = new();

	[JsonPropertyName("api_f_nowhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFNowhps { get; set; } = new();

	[JsonPropertyName("api_f_nowhps_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFNowhpsCombined { get; set; } = new();

	[JsonPropertyName("api_flavor_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiFlavorInfo> ApiFlavorInfo { get; set; } = new();

	[JsonPropertyName("api_formation")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFormation { get; set; } = new();

	[JsonPropertyName("api_hougeki1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiHougeki ApiHougeki1 { get; set; } = new();

	[JsonPropertyName("api_hougeki2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiHougeki? ApiHougeki2 { get; set; } = default!;

	[JsonPropertyName("api_hougeki3")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiHougeki ApiHougeki3 { get; set; } = new();

	[JsonPropertyName("api_hourai_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiHouraiFlag { get; set; } = new();

	[JsonPropertyName("api_injection_kouku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiInjectionKouku? ApiInjectionKouku { get; set; } = default!;

	[JsonPropertyName("api_kouku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiKouku ApiKouku { get; set; } = new();

	[JsonPropertyName("api_midnight_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMidnightFlag { get; set; } = default!;

	[JsonPropertyName("api_opening_atack")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiRaigekiClass? ApiOpeningAtack { get; set; } = default!;

	[JsonPropertyName("api_opening_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiOpeningFlag { get; set; } = default!;

	[JsonPropertyName("api_opening_taisen")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public object? ApiOpeningTaisen { get; set; } = default!;

	[JsonPropertyName("api_opening_taisen_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiOpeningTaisenFlag { get; set; } = default!;

	[JsonPropertyName("api_raigeki")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiRaigekiClass ApiRaigeki { get; set; } = new();

	[JsonPropertyName("api_search")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiSearch { get; set; } = new();

	[JsonPropertyName("api_ship_ke")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipKe { get; set; } = new();

	[JsonPropertyName("api_ship_ke_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipKeCombined { get; set; } = new();

	[JsonPropertyName("api_ship_lv")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipLv { get; set; } = new();

	[JsonPropertyName("api_ship_lv_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipLvCombined { get; set; } = new();

	[JsonPropertyName("api_stage_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiStageFlag { get; set; } = new();

	[JsonPropertyName("api_support_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSupportFlag { get; set; } = default!;

	[JsonPropertyName("api_support_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiSupportInfo? ApiSupportInfo { get; set; } = default!;

	[JsonPropertyName("api_xal01")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiXal01 { get; set; } = default!;
}
