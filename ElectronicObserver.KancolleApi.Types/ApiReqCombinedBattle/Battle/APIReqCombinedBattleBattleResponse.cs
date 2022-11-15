using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battle;

public class ApiReqCombinedBattleBattleResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_air_base_attack")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<ApiAirBaseAttack>? ApiAirBaseAttack { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_deck_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiDeckId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_eParam")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiEParam { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_eSlot")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiESlot { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_e_maxhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEMaxhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_e_nowhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiENowhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_escape_idx_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiEscapeIdxCombined { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_fParam")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiFParam { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_fParam_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiFParamCombined { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_f_maxhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_f_maxhps_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFMaxhpsCombined { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_f_nowhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFNowhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_f_nowhps_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFNowhpsCombined { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_flavor_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<ApiFlavorInfo>? ApiFlavorInfo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_formation")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFormation { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_hougeki1")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiHougeki1? ApiHougeki1 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_hougeki2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiHougeki1? ApiHougeki2 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_hougeki3")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiHougeki1? ApiHougeki3 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_hourai_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiHouraiFlag { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_injection_kouku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiInjectionKouku? ApiInjectionKouku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_kouku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiKouku ApiKouku { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_midnight_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMidnightFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_opening_atack")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiOpeningAtack? ApiOpeningAtack { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_opening_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiOpeningFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_opening_taisen")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiHougeki1? ApiOpeningTaisen { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_opening_taisen_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiOpeningTaisenFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_raigeki")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiRaigekiClass? ApiRaigeki { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_search")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiSearch { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_ke")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipKe { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_lv")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipLv { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_stage_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiStageFlag { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_support_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSupportFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_support_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiSupportInfo? ApiSupportInfo { get; set; } = default!;
}
