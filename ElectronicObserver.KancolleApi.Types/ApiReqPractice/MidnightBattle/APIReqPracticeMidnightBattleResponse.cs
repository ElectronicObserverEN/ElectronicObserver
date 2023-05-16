﻿using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.MidnightBattle;

public class ApiReqPracticeMidnightBattleResponse : IBattleApiResponse
{
	[JsonPropertyName("api_deck_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_eParam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiEParam { get; set; } = new();

	[JsonPropertyName("api_escape_idx")]
	public List<int>? ApiEscapeIdx { get; set; }

	[JsonPropertyName("api_smoke_type")]
	public int? ApiSmokeType { get; set; }

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

	[JsonPropertyName("api_friendly_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; }

	[JsonPropertyName("api_friendly_battle")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; }

	[JsonPropertyName("api_f_maxhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[JsonPropertyName("api_f_nowhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFNowhps { get; set; } = new();

	[JsonPropertyName("api_flare_pos")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFlarePos { get; set; } = new();

	[JsonPropertyName("api_formation")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFormation { get; set; } = new();

	[JsonPropertyName("api_hougeki")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiHougeki ApiHougeki { get; set; } = new();

	[JsonPropertyName("api_ship_ke")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipKe { get; set; } = new();

	[JsonPropertyName("api_ship_lv")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipLv { get; set; } = new();

	[JsonPropertyName("api_touch_plane")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiTouchPlane { get; set; } = new();
}
