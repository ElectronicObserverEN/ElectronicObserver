using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battle;

public class ApiReqCombinedBattleBattleResponse : IDayBattleApiResponse, ISupportApiResponse
{
	[JsonPropertyName("api_air_base_attack")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<ApiAirBaseAttack>? ApiAirBaseAttack { get; set; }

	[JsonPropertyName("api_air_base_injection")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiAirBaseInjection? ApiAirBaseInjection { get; set; }

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

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_e_maxhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<object> ApiEMaxhps { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_e_nowhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<object> ApiENowhps { get; set; } = new();

	[JsonPropertyName("api_escape_idx_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiEscapeIdxCombined { get; set; }

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

	[JsonPropertyName("api_fParam_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiFParamCombined { get; set; } = new();

	[JsonPropertyName("api_f_maxhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[JsonPropertyName("api_f_maxhps_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFMaxhpsCombined { get; set; } = new();

	[JsonPropertyName("api_f_nowhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFNowhps { get; set; } = new();

	[JsonPropertyName("api_f_nowhps_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFNowhpsCombined { get; set; } = new();

	[JsonPropertyName("api_flavor_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<ApiFlavorInfo>? ApiFlavorInfo { get; set; }

	[JsonPropertyName("api_formation")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFormation { get; set; } = new();

	[JsonPropertyName("api_hougeki1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiHougeki1? ApiHougeki1 { get; set; } 

	[JsonPropertyName("api_hougeki2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiHougeki1? ApiHougeki2 { get; set; }

	[JsonPropertyName("api_hougeki3")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiHougeki1? ApiHougeki3 { get; set; }

	[JsonPropertyName("api_hourai_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiHouraiFlag { get; set; } = new();

	[JsonPropertyName("api_injection_kouku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiInjectionKouku? ApiInjectionKouku { get; set; }

	[JsonPropertyName("api_kouku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiKouku ApiKouku { get; set; } = new();

	[JsonPropertyName("api_midnight_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMidnightFlag { get; set; }

	[JsonPropertyName("api_opening_atack")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiRaigekiClass? ApiOpeningAtack { get; set; }

	[JsonPropertyName("api_opening_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiOpeningFlag { get; set; }

	[JsonPropertyName("api_opening_taisen")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiHougeki1? ApiOpeningTaisen { get; set; }

	[JsonPropertyName("api_opening_taisen_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiOpeningTaisenFlag { get; set; }

	[JsonPropertyName("api_raigeki")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiRaigekiClass? ApiRaigeki { get; set; }

	[JsonPropertyName("api_search")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<DetectionType> ApiSearch { get; set; } = new();

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

	[JsonPropertyName("api_support_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public SupportType ApiSupportFlag { get; set; }

	[JsonPropertyName("api_support_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiSupportInfo? ApiSupportInfo { get; set; }
}
