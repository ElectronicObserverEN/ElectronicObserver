using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;

public class ApiReqCombinedBattleEcBattleResponse : ICombinedDayBattleApiResponse, IEnemyCombinedFleetBattle
{
	[JsonPropertyName("api_air_base_attack")]
	public List<ApiAirBaseAttack>? ApiAirBaseAttack { get; set; }

	[JsonPropertyName("api_air_base_injection")]
	public ApiAirBaseInjection? ApiAirBaseInjection { get; set; }

	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_eParam")]
	[Required]
	public List<List<int>> ApiEParam { get; set; } = new();

	[JsonPropertyName("api_escape_idx")]
	public List<int>? ApiEscapeIdx { get; set; }

	[JsonPropertyName("api_smoke_type")]
	public int? ApiSmokeType { get; set; }

	[JsonPropertyName("api_eParam_combined")]
	[Required]
	public List<List<int>> ApiEParamCombined { get; set; } = new();

	[JsonPropertyName("api_eSlot")]
	[Required]
	public List<List<int>> ApiESlot { get; set; } = new();

	[JsonPropertyName("api_eSlot_combined")]
	[Required]
	public List<List<int>> ApiESlotCombined { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_e_maxhps")]
	[Required]
	public List<object> ApiEMaxhps { get; set; } = new();

	[JsonPropertyName("api_e_maxhps_combined")]
	[Required]
	public List<int> ApiEMaxhpsCombined { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_e_nowhps")]
	[Required]
	public List<object> ApiENowhps { get; set; } = new();

	[JsonPropertyName("api_e_nowhps_combined")]
	[Required]
	public List<int> ApiENowhpsCombined { get; set; } = new();

	[JsonPropertyName("api_fParam")]
	[Required]
	public List<List<int>> ApiFParam { get; set; } = new();

	[JsonPropertyName("api_friendly_info")]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; }

	[JsonPropertyName("api_friendly_battle")]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; }

	[JsonPropertyName("api_f_maxhps")]
	[Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[JsonPropertyName("api_f_nowhps")]
	[Required]
	public List<int> ApiFNowhps { get; set; } = new();

	[JsonPropertyName("api_formation")]
	[Required]
	public List<int> ApiFormation { get; set; } = new();

	[JsonPropertyName("api_friendly_kouku")]
	public ApiKouku? ApiFriendlyKouku { get; set; }

	[JsonPropertyName("api_flavor_info")]
	[Required]
	public List<ApiFlavorInfo>? ApiFlavorInfo { get; set; }

	[JsonPropertyName("api_hougeki1")]
	[Required]
	public ApiHougeki1 ApiHougeki1 { get; set; } = new();

	[JsonPropertyName("api_hougeki2")]
	[Required]
	public ApiHougeki1 ApiHougeki2 { get; set; } = new();

	[JsonPropertyName("api_hougeki3")]
	[Required]
	public ApiHougeki1 ApiHougeki3 { get; set; } = new();

	[JsonPropertyName("api_hourai_flag")]
	[Required]
	public List<int> ApiHouraiFlag { get; set; } = new();

	[JsonPropertyName("api_injection_kouku")]
	public ApiInjectionKouku? ApiInjectionKouku { get; set; }

	[JsonPropertyName("api_kouku")]
	[Required]
	public ApiKouku ApiKouku { get; set; } = new();

	[JsonPropertyName("api_midnight_flag")]
	public int ApiMidnightFlag { get; set; } = default!;

	[JsonPropertyName("api_opening_atack")]
	public ApiRaigekiClass ApiOpeningAtack { get; set; } = default!;

	[JsonPropertyName("api_opening_flag")]
	public int ApiOpeningFlag { get; set; } = default!;

	[JsonPropertyName("api_opening_taisen")]
	public ApiHougeki1? ApiOpeningTaisen { get; set; } = default!;

	[JsonPropertyName("api_opening_taisen_flag")]
	public int ApiOpeningTaisenFlag { get; set; } = default!;

	[JsonPropertyName("api_raigeki")]
	[Required]
	public ApiRaigekiClass ApiRaigeki { get; set; } = new();

	[JsonPropertyName("api_search")]
	[Required]
	public List<DetectionType> ApiSearch { get; set; } = new();

	[JsonPropertyName("api_ship_ke")]
	[Required]
	public List<int> ApiShipKe { get; set; } = new();

	[JsonPropertyName("api_ship_ke_combined")]
	[Required]
	public List<int> ApiShipKeCombined { get; set; } = new();

	[JsonPropertyName("api_ship_lv")]
	[Required]
	public List<int> ApiShipLv { get; set; } = new();

	[JsonPropertyName("api_ship_lv_combined")]
	[Required]
	public List<int> ApiShipLvCombined { get; set; } = new();

	[JsonPropertyName("api_stage_flag")]
	[Required]
	public List<int> ApiStageFlag { get; set; } = new();

	[JsonPropertyName("api_support_flag")]
	public SupportType ApiSupportFlag { get; set; }

	[JsonPropertyName("api_support_info")]
	public ApiSupportInfo? ApiSupportInfo { get; set; }
}
