using ElectronicObserver.KancolleApi.Types.ApiGetMember.Ndock;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiPort.Port;

public class ApiPortPortResponse
{
	[JsonPropertyName("api_basic")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiPortBasic ApiBasic { get; set; } = new();

	[JsonPropertyName("api_c_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiCFlag { get; set; } = default!;

	[JsonPropertyName("api_combined_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiCombinedFlag { get; set; } = default!;

	[JsonPropertyName("api_deck_port")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiDeckPort> ApiDeckPort { get; set; } = new();

	[JsonPropertyName("api_dest_ship_slot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDestShipSlot { get; set; } = default!;

	[JsonPropertyName("api_event_object")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiEventObject? ApiEventObject { get; set; } = default!;

	[JsonPropertyName("api_log")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiLog> ApiLog { get; set; } = new();

	[JsonPropertyName("api_material")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMaterial> ApiMaterial { get; set; } = new();

	[JsonPropertyName("api_ndock")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiGetMemberNdockResponse> ApiNdock { get; set; } = new();

	[JsonPropertyName("api_p_bgm_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPBgmId { get; set; } = default!;

	[JsonPropertyName("api_parallel_quest_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiParallelQuestCount { get; set; } = default!;

	[JsonPropertyName("api_plane_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiPortPlaneInfo? ApiPlaneInfo { get; set; } = default!;

	[JsonPropertyName("api_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiShip> ApiShip { get; set; } = new();
}
