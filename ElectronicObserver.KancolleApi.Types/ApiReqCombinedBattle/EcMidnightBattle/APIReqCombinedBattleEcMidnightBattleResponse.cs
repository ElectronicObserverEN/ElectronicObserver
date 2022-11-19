using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;

public class ApiReqCombinedBattleEcMidnightBattleResponse
{
	[JsonPropertyName("api_active_deck")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiActiveDeck { get; set; } = new();

	[JsonPropertyName("api_deck_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_eParam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiEParam { get; set; } = new();

	[JsonPropertyName("api_eParam_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiEParamCombined { get; set; } = new();

	[JsonPropertyName("api_eSlot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiESlot { get; set; } = new();

	[JsonPropertyName("api_eSlot_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiESlotCombined { get; set; } = new();

	[JsonPropertyName("api_e_maxhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEMaxhps { get; set; } = new();

	[JsonPropertyName("api_e_maxhps_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiEMaxhpsCombined { get; set; } = new();

	[JsonPropertyName("api_e_nowhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiENowhps { get; set; } = new();

	[JsonPropertyName("api_e_nowhps_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiENowhpsCombined { get; set; } = new();

	[JsonPropertyName("api_escape_idx")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiEscapeIdx { get; set; } = default!;

	[JsonPropertyName("api_escape_idx_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiEscapeIdxCombined { get; set; } = default!;

	[JsonPropertyName("api_fParam")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<List<int>> ApiFParam { get; set; } = new();

	[JsonPropertyName("api_fParam_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<List<int>>? ApiFParamCombined { get; set; } = default!;

	[JsonPropertyName("api_f_maxhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[JsonPropertyName("api_f_maxhps_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiFMaxhpsCombined { get; set; } = default!;

	[JsonPropertyName("api_f_nowhps")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFNowhps { get; set; } = new();

	[JsonPropertyName("api_f_nowhps_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiFNowhpsCombined { get; set; } = default!;

	[JsonPropertyName("api_flare_pos")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFlarePos { get; set; } = new();

	[JsonPropertyName("api_formation")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFormation { get; set; } = new();

	[JsonPropertyName("api_friendly_battle")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; } = default!;

	[JsonPropertyName("api_friendly_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; } = default!;

	[JsonPropertyName("api_hougeki")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiHougeki ApiHougeki { get; set; } = new();

	[JsonPropertyName("api_ship_ke")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipKe { get; set; } = new();

	[JsonPropertyName("api_ship_ke_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipKeCombined { get; set; } = new();

	[JsonPropertyName("api_ship_lv")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipLv { get; set; } = new();

	[JsonPropertyName("api_ship_lv_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShipLvCombined { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[JsonPropertyName("api_touch_plane")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<object> ApiTouchPlane { get; set; } = new();
}
