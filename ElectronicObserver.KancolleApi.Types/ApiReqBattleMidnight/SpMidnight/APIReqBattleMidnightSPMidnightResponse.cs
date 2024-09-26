using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;

public class ApiReqBattleMidnightSpMidnightResponse : INightOnlyBattleApiResponse
{
	/// <inheritdoc />
	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_eParam")]
	public List<List<int>> ApiEParam { get; set; } = new();

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

	/// <inheritdoc />
	[JsonPropertyName("api_eSlot")]
	public List<List<int>> ApiESlot { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_e_maxhps")]
	public List<object> ApiEMaxhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_e_nowhps")]
	public List<object> ApiENowhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_fParam")]
	public List<List<int>> ApiFParam { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_friendly_info")]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_friendly_battle")]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_f_maxhps")]
	public List<int> ApiFMaxhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_f_nowhps")]
	public List<int> ApiFNowhps { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_flare_pos")]
	public List<int> ApiFlarePos { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_formation")]
	public List<int> ApiFormation { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_balloon_cell")]
	public int? ApiBalloonCell { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_hougeki")]
	public ApiHougeki? ApiHougeki { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_n_support_flag")]
	public SupportType ApiNSupportFlag { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_n_support_info")]
	public ApiSupportInfo? ApiNSupportInfo { get; set; }

	/// <inheritdoc />
	[JsonPropertyName("api_ship_ke")]
	public List<int> ApiShipKe { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_ship_lv")]
	public List<int> ApiShipLv { get; set; } = new();

	/// <inheritdoc />
	[JsonPropertyName("api_touch_plane")]
	public List<object> ApiTouchPlane { get; set; } = new();
}
