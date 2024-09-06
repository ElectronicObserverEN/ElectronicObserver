using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.NightToDay;

public class ApiReqSortieNightToDayResponse : IDayFromNightBattleApiResponse
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
	[JsonPropertyName("api_fParam")]
	public List<List<int>> ApiFParam { get; set; } = new();

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

	/// <inheritdoc />
	[JsonPropertyName("api_touch_plane")]
	public List<object> ApiTouchPlane { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_flare_pos")]
	public List<int> ApiFlarePos { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_n_support_flag")]
	public SupportType ApiNSupportFlag { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_n_support_info")]
	public ApiSupportInfo? ApiNSupportInfo { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_n_hougeki1")]
	public ApiHougeki? ApiNHougeki1 { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_n_hougeki2")]
	public ApiHougeki? ApiNHougeki2 { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_day_flag")]
	public int ApiDayFlag { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_search")]
	public List<DetectionType> ApiSearch { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_air_base_injection")]
	public ApiAirBaseInjection? ApiAirBaseInjection { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_injection_kouku")]
	public ApiInjectionKouku? ApiInjectionKouku { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_air_base_attack")]
	public List<ApiAirBaseAttack>? ApiAirBaseAttack { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_friendly_info")]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_friendly_battle")]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_friendly_kouku")]
	public ApiKouku? ApiFriendlyKouku { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_support_flag")]
	public SupportType ApiSupportFlag { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_support_info")]
	public ApiSupportInfo? ApiSupportInfo { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_stage_flag")]
	public List<int> ApiStageFlag { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_kouku")]
	public ApiKouku? ApiKouku { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_opening_taisen_flag")]
	public int ApiOpeningTaisenFlag { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_opening_taisen")]
	public ApiHougeki1? ApiOpeningTaisen { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_opening_flag")]
	public int ApiOpeningFlag { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_opening_atack")]
	public ApiPhaseOpeningTorpedo? ApiOpeningAtack { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_hourai_flag")]
	public List<int> ApiHouraiFlag { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_hougeki1")]
	public ApiHougeki1? ApiHougeki1 { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_hougeki2")]
	public ApiHougeki1? ApiHougeki2 { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_raigeki")]
	public ApiRaigekiClass? ApiRaigeki { get; set; }

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
}
