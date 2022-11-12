using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;

public class ApiReqCombinedBattleEcMidnightBattleResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_active_deck")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiActiveDeck { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_deck_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiDeckId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_eParam")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiEParam { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_eParam_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiEParamCombined { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_eSlot")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiESlot { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_eSlot_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiESlotCombined { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_e_maxhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEMaxhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_e_maxhps_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEMaxhpsCombined { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_e_nowhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiENowhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_e_nowhps_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiENowhpsCombined { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_escape_idx")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiEscapeIdx { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_escape_idx_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiEscapeIdxCombined { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_fParam")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiFParam { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_fParam_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<List<int>>? ApiFParamCombined { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_f_maxhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_f_maxhps_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiFMaxhpsCombined { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_f_nowhps")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFNowhps { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_f_nowhps_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiFNowhpsCombined { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_flare_pos")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFlarePos { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_formation")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFormation { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_friendly_battle")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_friendly_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_hougeki")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiReqCombinedBattleEcMidnightBattleResponseApiHougeki ApiHougeki { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_ke")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipKe { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_ke_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipKeCombined { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_lv")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipLv { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_lv_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShipLvCombined { get; set; } = new();

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	[System.Text.Json.Serialization.JsonPropertyName("api_touch_plane")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<object> ApiTouchPlane { get; set; } = new();
}
