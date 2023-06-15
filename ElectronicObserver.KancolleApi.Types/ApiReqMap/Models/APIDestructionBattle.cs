using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiDestructionBattle : IFirstBattleApiResponse
{
	[JsonPropertyName("api_air_base_attack")]
	public ApiAirBaseRaid? ApiAirBaseAttack { get; set; }

	[JsonPropertyName("api_eSlot")]
	[Required]
	public List<List<int>> ApiESlot { get; set; } = new();

	[JsonPropertyName("api_eParam")]
	public List<List<int>> ApiEParam { get; set; } = new();

	[JsonPropertyName("api_escape_idx")]
	public List<int>? ApiEscapeIdx { get; set; }

	[JsonPropertyName("api_smoke_type")]
	public int? ApiSmokeType { get; set; }

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

	[JsonPropertyName("api_f_maxhps")]
	[Required]
	public List<int> ApiFMaxhps { get; set; } = new();

	[JsonPropertyName("api_fParam")]
	[Required]
	public List<List<int>> ApiFParam { get; set; } = new();

	[JsonPropertyName("api_friendly_info")]
	public ApiFriendlyInfo? ApiFriendlyInfo { get; set; }

	[JsonPropertyName("api_friendly_battle")]
	public ApiFriendlyBattle? ApiFriendlyBattle { get; set; }

	[JsonPropertyName("api_f_nowhps")]
	[Required]
	public List<int> ApiFNowhps { get; set; } = new();

	public int ApiDeckId { get; set; }

	[JsonPropertyName("api_formation")]
	[Required]
	public List<int> ApiFormation { get; set; } = new();

	[JsonPropertyName("api_lost_kind")]
	public int ApiLostKind { get; set; } = default!;

	[JsonPropertyName("api_ship_ke")]
	[Required]
	public List<int> ApiShipKe { get; set; } = new();

	[JsonPropertyName("api_ship_lv")]
	[Required]
	public List<int> ApiShipLv { get; set; } = new();
}
