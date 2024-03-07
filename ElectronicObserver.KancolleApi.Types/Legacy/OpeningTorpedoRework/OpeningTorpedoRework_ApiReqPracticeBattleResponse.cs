using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Legacy.OpeningTorpedoRework;

public class OpeningTorpedoRework_ApiReqPracticeBattleResponse : IFirstBattleApiResponse
{
	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; }

	[JsonPropertyName("api_eParam")]
	public List<List<int>> ApiEParam { get; set; } = [];

	[JsonPropertyName("api_escape_idx")]
	public List<int>? ApiEscapeIdx { get; set; }

	[JsonPropertyName("api_xal01")]
	public int? ApiXal01 { get; set; }

	[JsonPropertyName("api_combat_ration")]
	public List<int>? ApiCombatRation { get; set; }

	[JsonPropertyName("api_smoke_type")]
	public int? ApiSmokeType { get; set; }

	[JsonPropertyName("api_eSlot")]
	public List<List<int>> ApiESlot { get; set; } = [];

	[JsonPropertyName("api_e_maxhps")]
	public List<object> ApiEMaxhps { get; set; } = [];

	[JsonPropertyName("api_e_nowhps")]
	public List<object> ApiENowhps { get; set; } = [];

	[JsonPropertyName("api_fParam")]
	public List<List<int>> ApiFParam { get; set; } = [];

	[JsonPropertyName("api_friendly_info")]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; }

	[JsonPropertyName("api_friendly_battle")]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; }

	[JsonPropertyName("api_f_maxhps")]
	public List<int> ApiFMaxhps { get; set; } = [];

	[JsonPropertyName("api_f_nowhps")]
	public List<int> ApiFNowhps { get; set; } = [];

	[JsonPropertyName("api_formation")]
	public List<int> ApiFormation { get; set; } = [];

	[JsonPropertyName("api_hougeki1")]
	public ApiHougeki1? ApiHougeki1 { get; set; }

	[JsonPropertyName("api_hougeki2")]
	public ApiHougeki1? ApiHougeki2 { get; set; }

	[JsonPropertyName("api_hourai_flag")]
	public List<int> ApiHouraiFlag { get; set; } = [];

	[JsonPropertyName("api_injection_kouku")]
	public ApiInjectionKouku? ApiInjectionKouku { get; set; }

	[JsonPropertyName("api_kouku")]
	public ApiKouku ApiKouku { get; set; } = new();

	[JsonPropertyName("api_midnight_flag")]
	public int ApiMidnightFlag { get; set; }

	[JsonPropertyName("api_opening_atack")]
	public ApiRaigekiClass? ApiOpeningAtack { get; set; }

	[JsonPropertyName("api_opening_flag")]
	public int ApiOpeningFlag { get; set; }

	[JsonPropertyName("api_opening_taisen")]
	public ApiHougeki1? ApiOpeningTaisen { get; set; }

	[JsonPropertyName("api_opening_taisen_flag")]
	public int ApiOpeningTaisenFlag { get; set; }

	[JsonPropertyName("api_raigeki")]
	public ApiRaigekiClass? ApiRaigeki { get; set; }

	[JsonPropertyName("api_search")]
	public List<int> ApiSearch { get; set; } = [];

	[JsonPropertyName("api_ship_ke")]
	public List<int> ApiShipKe { get; set; } = [];

	[JsonPropertyName("api_ship_lv")]
	public List<int> ApiShipLv { get; set; } = [];

	[JsonPropertyName("api_stage_flag")]
	public List<int> ApiStageFlag { get; set; } = [];
}
