﻿using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.MidnightBattle;

public class ApiReqPracticeMidnightBattleResponse : IBattleApiResponse, INightGearApiResponse
{
	/// <inheritdoc />
	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_eParam")]
	public List<List<int>> ApiEParam { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_escape_idx")]
	public List<int>? ApiEscapeIdx { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_xal01")]
	public int? ApiXal01 { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_combat_ration")]
	public List<int>? ApiCombatRation { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_smoke_type")]
	public int? ApiSmokeType { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_eSlot")]
	public List<List<int>> ApiESlot { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_e_maxhps")]
	public List<object> ApiEMaxhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_e_nowhps")]
	public List<object> ApiENowhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_fParam")]
	public List<List<int>> ApiFParam { get; set; } = new();

	[JsonPropertyName("api_friendly_info")]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; }

	[JsonPropertyName("api_friendly_battle")]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_f_maxhps")]
	public List<int> ApiFMaxhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_f_nowhps")]
	public List<int> ApiFNowhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_flare_pos")]
	public List<int> ApiFlarePos { get; set; } = new();

	[JsonPropertyName("api_formation")]
	public List<int> ApiFormation { get; set; } = new();

	[JsonPropertyName("api_hougeki")]
	public ApiHougeki? ApiHougeki { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_ship_ke")]
	public List<int> ApiShipKe { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_ship_lv")]
	public List<int> ApiShipLv { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_touch_plane")]
	public List<object> ApiTouchPlane { get; set; } = new();
}
