using ElectronicObserver.KancolleApi.Types.ApiGetMember.Ndock;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiPort.Port;

public class ApiPortPortResponse
{
	[JsonPropertyName("api_basic")]
	public ApiPortBasic ApiBasic { get; set; } = new();

	[JsonPropertyName("api_c_flag")]
	public int? ApiCFlag { get; set; } = default!;

	[JsonPropertyName("api_combined_flag")]
	public int? ApiCombinedFlag { get; set; } = default!;

	[JsonPropertyName("api_deck_port")]
	public List<ApiDeckPort> ApiDeckPort { get; set; } = new();

	[JsonPropertyName("api_dest_ship_slot")]
	public int ApiDestShipSlot { get; set; } = default!;

	[JsonPropertyName("api_event_object")]
	public ApiEventObject? ApiEventObject { get; set; } = default!;

	[JsonPropertyName("api_log")]
	public List<ApiLog> ApiLog { get; set; } = new();

	[JsonPropertyName("api_material")]
	public List<ApiMaterial> ApiMaterial { get; set; } = new();

	[JsonPropertyName("api_ndock")]
	public List<ApiGetMemberNdockResponse> ApiNdock { get; set; } = new();

	[JsonPropertyName("api_p_bgm_id")]
	public int ApiPBgmId { get; set; } = default!;

	[JsonPropertyName("api_parallel_quest_count")]
	public int ApiParallelQuestCount { get; set; } = default!;

	[JsonPropertyName("api_plane_info")]
	public ApiPortPlaneInfo? ApiPlaneInfo { get; set; } = default!;

	[JsonPropertyName("api_ship")]
	public List<ApiShip> ApiShip { get; set; } = new();
}
