﻿using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.Battle;

public class ApiReqPracticeBattleResponse : IBattleApiResponse
{
	[JsonPropertyName("api_deck_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_eParam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiEParam { get; set; } = new();

	public List<int>? ApiEscapeIdx { get; set; }

	[JsonPropertyName("api_eSlot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiESlot { get; set; } = new();

	[JsonPropertyName("api_e_maxhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<object> ApiEMaxhps { get; set; } = new();

	[JsonPropertyName("api_e_nowhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<object> ApiENowhps { get; set; } = new();

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

	[JsonPropertyName("api_hougeki1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiHougeki1? ApiHougeki1 { get; set; } = default!;

	[JsonPropertyName("api_hougeki2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiHougeki1? ApiHougeki2 { get; set; } = default!;

	[JsonPropertyName("api_hougeki3")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public object? ApiHougeki3 { get; set; } = default!;

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
	public ApiRaigekiClass ApiOpeningAtack { get; set; } = default!;

	[JsonPropertyName("api_opening_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiOpeningFlag { get; set; } = default!;

	[JsonPropertyName("api_opening_taisen")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiOpeningTaisen ApiOpeningTaisen { get; set; } = default!;

	[JsonPropertyName("api_opening_taisen_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiOpeningTaisenFlag { get; set; } = default!;

	[JsonPropertyName("api_raigeki")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiRaigekiClass ApiRaigeki { get; set; } = default!;

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
