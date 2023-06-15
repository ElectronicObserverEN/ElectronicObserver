using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.BattleWater;

public class ApiReqCombinedBattleBattleWaterResponse : ICombinedDayBattleApiResponse, IPlayerCombinedFleetBattle
{
	[JsonPropertyName("api_air_base_attack")]
	public List<ApiAirBaseAttack>? ApiAirBaseAttack { get; set; }

	[JsonPropertyName("api_air_base_injection")]
	public ApiAirBaseInjection? ApiAirBaseInjection { get; set; }

	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; }

	[JsonPropertyName("api_eParam")]
	[Required]
	public List<List<int>> ApiEParam { get; set; } = new();

	[JsonPropertyName("api_eSlot")]
	[Required]
	public List<List<int>> ApiESlot { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_e_maxhps")]
	[Required]
	public List<object> ApiEMaxhps { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_e_nowhps")]
	[Required]
	public List<object> ApiENowhps { get; set; } = new();

	[JsonPropertyName("api_escape_idx")]
	public List<int>? ApiEscapeIdx { get; set; }

	[JsonPropertyName("api_smoke_type")]
	public int? ApiSmokeType { get; set; }

	[JsonPropertyName("api_escape_idx_combined")]
	public List<int>? ApiEscapeIdxCombined { get; set; }

	[JsonPropertyName("api_fParam")]
	[Required]
	public List<List<int>> ApiFParam { get; set; } = new();

	[JsonPropertyName("api_friendly_info")]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; }

	[JsonPropertyName("api_friendly_battle")]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; }

	[JsonPropertyName("api_fParam_combined")]
	[Required]
	public List<List<int>> ApiFParamCombined { get; set; } = new();

	[JsonPropertyName("api_f_maxhps")]
	[Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[JsonPropertyName("api_f_maxhps_combined")]
	[Required]
	public List<int> ApiFMaxhpsCombined { get; set; } = new();

	[JsonPropertyName("api_f_nowhps")]
	[Required]
	public List<int> ApiFNowhps { get; set; } = new();

	[JsonPropertyName("api_f_nowhps_combined")]
	[Required]
	public List<int> ApiFNowhpsCombined { get; set; } = new();

	[JsonPropertyName("api_formation")]
	[Required]
	public List<int> ApiFormation { get; set; } = new();

	[JsonPropertyName("api_friendly_kouku")]
	public ApiKouku? ApiFriendlyKouku { get; set; }

	[JsonPropertyName("api_flavor_info")]
	public List<ApiFlavorInfo>? ApiFlavorInfo { get; set; }

	[JsonPropertyName("api_hougeki1")]
	public ApiHougeki1? ApiHougeki1 { get; set; }

	[JsonPropertyName("api_hougeki2")]
	public ApiHougeki1? ApiHougeki2 { get; set; }

	[JsonPropertyName("api_hougeki3")]
	public ApiHougeki1? ApiHougeki3 { get; set; }

	[JsonPropertyName("api_hourai_flag")]
	[Required]
	public List<int> ApiHouraiFlag { get; set; } = new();

	[JsonPropertyName("api_injection_kouku")]
	public ApiInjectionKouku? ApiInjectionKouku { get; set; }

	[JsonPropertyName("api_kouku")]
	[Required]
	public ApiKouku ApiKouku { get; set; } = new();

	[JsonPropertyName("api_midnight_flag")]
	public int ApiMidnightFlag { get; set; }

	[JsonPropertyName("api_opening_atack")]
	public ApiRaigekiClass ApiOpeningAtack { get; set; }

	[JsonPropertyName("api_opening_flag")]
	public int ApiOpeningFlag { get; set; }

	[JsonPropertyName("api_opening_taisen")]
	public ApiHougeki1 ApiOpeningTaisen { get; set; }

	[JsonPropertyName("api_opening_taisen_flag")]
	public int ApiOpeningTaisenFlag { get; set; }

	[JsonPropertyName("api_raigeki")]
	public ApiRaigekiClass? ApiRaigeki { get; set; }

	[JsonPropertyName("api_search")]
	[Required]
	public List<DetectionType> ApiSearch { get; set; } = new();

	[JsonPropertyName("api_ship_ke")]
	[Required]
	public List<int> ApiShipKe { get; set; } = new();

	[JsonPropertyName("api_ship_lv")]
	[Required]
	public List<int> ApiShipLv { get; set; } = new();

	[JsonPropertyName("api_stage_flag")]
	[Required]
	public List<int> ApiStageFlag { get; set; } = new();

	[JsonPropertyName("api_support_flag")]
	public SupportType ApiSupportFlag { get; set; }

	[JsonPropertyName("api_support_info")]
	public ApiSupportInfo? ApiSupportInfo { get; set; }
}
