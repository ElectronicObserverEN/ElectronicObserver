using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdAirbattle;

public class ApiReqCombinedBattleLdAirbattleResponse : IAirBattleApiResponse, IPlayerCombinedFleetBattle
{
	[JsonPropertyName("api_air_base_injection")]
	public ApiAirBaseInjection? ApiAirBaseInjection { get; set; }

	[JsonPropertyName("api_air_base_attack")]
	public List<ApiAirBaseAttack>? ApiAirBaseAttack { get; set; }

	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; }

	[JsonPropertyName("api_eParam")]
	public List<List<int>> ApiEParam { get; set; } = new();

	[JsonPropertyName("api_eSlot")]
	public List<List<int>> ApiESlot { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_e_maxhps")]
	public List<object> ApiEMaxhps { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_e_nowhps")]
	public List<object> ApiENowhps { get; set; } = new();

	[JsonPropertyName("api_escape_idx")]
	public List<int>? ApiEscapeIdx { get; set; }

	[JsonPropertyName("api_xal01")]
	public int? ApiXal01 { get; set; }

	[JsonPropertyName("api_smoke_type")]
	public int? ApiSmokeType { get; set; }

	[JsonPropertyName("api_escape_idx_combined")]
	public List<int>? ApiEscapeIdxCombined { get; set; }

	[JsonPropertyName("api_fParam")]
	public List<List<int>> ApiFParam { get; set; } = new();

	[JsonPropertyName("api_friendly_info")]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; }

	[JsonPropertyName("api_friendly_battle")]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; }

	[JsonPropertyName("api_fParam_combined")]
	public List<List<int>> ApiFParamCombined { get; set; } = new();

	[JsonPropertyName("api_f_maxhps")]
	public List<int> ApiFMaxhps { get; set; } = new();

	[JsonPropertyName("api_f_maxhps_combined")]
	public List<int> ApiFMaxhpsCombined { get; set; } = new();

	[JsonPropertyName("api_f_nowhps")]
	public List<int> ApiFNowhps { get; set; } = new();

	[JsonPropertyName("api_f_nowhps_combined")]
	public List<int> ApiFNowhpsCombined { get; set; } = new();

	[JsonPropertyName("api_formation")]
	public List<int> ApiFormation { get; set; } = new();

	[JsonPropertyName("api_injection_kouku")]
	public ApiInjectionKouku? ApiInjectionKouku { get; set; }

	[JsonPropertyName("api_kouku")]
	public ApiKouku? ApiKouku { get; set; } = new();

	[JsonPropertyName("api_midnight_flag")]
	public int ApiMidnightFlag { get; set; }

	[JsonPropertyName("api_search")]
	public List<DetectionType> ApiSearch { get; set; } = new();

	[JsonPropertyName("api_ship_ke")]
	public List<int> ApiShipKe { get; set; } = new();

	[JsonPropertyName("api_ship_lv")]
	public List<int> ApiShipLv { get; set; } = new();

	[JsonPropertyName("api_stage_flag")]
	public List<int> ApiStageFlag { get; set; } = new();

	[JsonPropertyName("api_friendly_kouku")]
	public ApiKouku? ApiFriendlyKouku { get; set; }
}
