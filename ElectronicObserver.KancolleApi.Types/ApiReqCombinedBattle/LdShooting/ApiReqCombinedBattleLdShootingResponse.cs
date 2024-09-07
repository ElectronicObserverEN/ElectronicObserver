﻿using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdShooting;

public class ApiReqCombinedBattleLdShootingResponse : IRadarBattleApiResponse, IPlayerCombinedFleetBattle
{
	/// <inheritdoc />
	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_formation")]
	public List<int> ApiFormation { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_balloon_cell")]
	public int? ApiBalloonCell { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_f_nowhps")]
	public List<int> ApiFNowhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_f_maxhps")]
	public List<int> ApiFMaxhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_f_nowhps_combined")]
	public List<int> ApiFNowhpsCombined { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_f_maxhps_combined")]
	public List<int> ApiFMaxhpsCombined { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_fParam")]
	public List<List<int>> ApiFParam { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_fParam_combined")]
	public List<List<int>> ApiFParamCombined { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_ship_ke")]
	public List<int> ApiShipKe { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_ship_lv")]
	public List<int> ApiShipLv { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_e_nowhps")]
	public List<object> ApiENowhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_e_maxhps")]
	public List<object> ApiEMaxhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_eSlot")]
	public List<List<int>> ApiESlot { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_eParam")]
	public List<List<int>> ApiEParam { get; set; } = new();

	[JsonPropertyName("api_midnight_flag")]
	public int ApiMidnightFlag { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_air_base_injection")]
	public ApiAirBaseInjection? ApiAirBaseInjection { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_air_base_attack")]
	public List<ApiAirBaseAttack>? ApiAirBaseAttack { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_hougeki1")]
	public ApiHougeki1? ApiHougeki1 { get; set; }

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
	[JsonPropertyName("api_escape_idx_combined")]
	public List<int>? ApiEscapeIdxCombined { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_combat_ration_combined")]
	public List<int>? ApiCombatRationCombined { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_smoke_type")]
	public int? ApiSmokeType { get; set; }
}
