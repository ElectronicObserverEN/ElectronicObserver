﻿using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.Battle;

public class ApiReqPracticeBattleResponse
{
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

	[System.Text.Json.Serialization.JsonPropertyName("api_fParam")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiFParam { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_f_maxhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_f_nowhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFNowhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_formation")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFormation { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_hougeki1")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiHougeki1? ApiHougeki1 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_hougeki2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiHougeki1? ApiHougeki2 { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_hougeki3")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public object? ApiHougeki3 { get; set; } = default!;

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
	public ApiOpeningAtack ApiOpeningAtack { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_opening_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiOpeningFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_opening_taisen")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiOpeningTaisen ApiOpeningTaisen { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_opening_taisen_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiOpeningTaisenFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_raigeki")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public ApiRaigekiClass ApiRaigeki { get; set; } = default!;

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
}
