using ElectronicObserver.Core.Types;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Airbattle;

public class ApiReqCombinedBattleAirbattleResponse : IAirBattleApiResponse, ISupportApiResponse, IPlayerCombinedFleetBattle
{
	/// <inheritdoc />
	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_ship_ke")]
	public List<int> ApiShipKe { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_ship_lv")]
	public List<int> ApiShipLv { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_nowhps")]
	public List<int> ApiFNowhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_maxhps")]
	public List<int> ApiFMaxhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_nowhps_combined")]
	public List<int> ApiFNowhpsCombined { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_maxhps_combined")]
	public List<int> ApiFMaxhpsCombined { get; set; } = new();

	[JsonPropertyName("api_midnight_flag")]
	public int ApiMidnightFlag { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_eSlot")]
	public List<List<int>> ApiESlot { get; set; } = new();

	// api_eKyouka ?

	/// <inheritdoc />
	[JsonPropertyName("api_fParam")]
	public List<List<int>> ApiFParam { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_eParam")]
	public List<List<int>> ApiEParam { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_fParam_combined")]
	public List<List<int>> ApiFParamCombined { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_escape_idx")]
	public List<int>? ApiEscapeIdx { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_xal01")]
	public int? ApiXal01 { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_escape_idx_combined")]
	public List<int>? ApiEscapeIdxCombined { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_combat_ration_combined")]
	public List<int>? ApiCombatRationCombined { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_search")]
	public List<DetectionType> ApiSearch { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_formation")]
	public List<int> ApiFormation { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_balloon_cell")]
	public int? ApiBalloonCell { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_stage_flag")]
	public List<int> ApiStageFlag { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_kouku")]
	public ApiKouku? ApiKouku { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_support_flag")]
	public SupportType ApiSupportFlag { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_support_info")]
	public ApiSupportInfo? ApiSupportInfo { get; set; }

	[JsonPropertyName("api_stage_flag2")]
	public List<int> ApiStageFlag2 { get; set; } = new();

	[JsonPropertyName("api_kouku2")]
	public ApiKouku? ApiKouku2 { get; set; }

	// todo: these 2 aren't used?
	/// <inheritdoc />
	public List<object> ApiENowhps { get; set; } = new();

	/// <inheritdoc />
	public List<object> ApiEMaxhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_combat_ration")]
	public List<int>? ApiCombatRation { get; set; }

	/// <inheritdoc />
	public int? ApiSmokeType { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_air_base_injection")]
	public ApiAirBaseInjection? ApiAirBaseInjection { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_air_base_attack")]
	public List<ApiAirBaseAttack>? ApiAirBaseAttack { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_friendly_info")]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_friendly_kouku")]
	public ApiKouku? ApiFriendlyKouku { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_injection_kouku")]
	public ApiInjectionKouku? ApiInjectionKouku { get; set; }
}
