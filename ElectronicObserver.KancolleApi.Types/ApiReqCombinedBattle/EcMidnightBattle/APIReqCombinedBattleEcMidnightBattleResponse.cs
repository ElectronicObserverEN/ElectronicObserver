using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;

public class ApiReqCombinedBattleEcMidnightBattleResponse : ISecondNightBattleApiResponse, ICombinedNightBattleApiResponse
{
	[JsonPropertyName("api_active_deck")]
	[Required]
	public List<int> ApiActiveDeck { get; set; } = new();

	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_eParam")]
	[Required]
	public List<List<int>> ApiEParam { get; set; } = new();

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

	[JsonPropertyName("api_escape_idx")]
	public List<int>? ApiEscapeIdx { get; set; } = default!;

	[JsonPropertyName("api_smoke_type")]
	public int? ApiSmokeType { get; set; }

	[JsonPropertyName("api_escape_idx_combined")]
	public List<int>? ApiEscapeIdxCombined { get; set; } = default!;

	[JsonPropertyName("api_fParam")]
	[Required]
	public List<List<int>> ApiFParam { get; set; } = new();

	[JsonPropertyName("api_fParam_combined")]
	public List<List<int>>? ApiFParamCombined { get; set; } = default!;

	[JsonPropertyName("api_f_maxhps")]
	[Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[JsonPropertyName("api_f_maxhps_combined")]
	public List<int>? ApiFMaxhpsCombined { get; set; } = default!;

	[JsonPropertyName("api_f_nowhps")]
	[Required]
	public List<int> ApiFNowhps { get; set; } = new();

	[JsonPropertyName("api_f_nowhps_combined")]
	public List<int>? ApiFNowhpsCombined { get; set; } = default!;

	[JsonPropertyName("api_flare_pos")]
	[Required]
	public List<int> ApiFlarePos { get; set; } = new();

	[JsonPropertyName("api_formation")]
	[Required]
	public List<int> ApiFormation { get; set; } = new();

	[JsonPropertyName("api_friendly_battle")]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; }

	[JsonPropertyName("api_friendly_info")]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; }

	[JsonPropertyName("api_hougeki")]
	[Required]
	public ApiHougeki ApiHougeki { get; set; } = new();

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

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_touch_plane")]
	[Required]
	public List<object> ApiTouchPlane { get; set; } = new();
}
