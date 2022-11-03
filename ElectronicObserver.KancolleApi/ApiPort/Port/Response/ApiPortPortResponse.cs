using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiPort.Port.Response;

public class ApiPortPortResponse
{

	[JsonPropertyName("api_basic")]
	public ApiBasic? ApiBasic { get; set; }

	[JsonPropertyName("api_c_flag")]
	public int ApiCFlag { get; set; }

	[JsonPropertyName("api_combined_flag")]
	public int ApiCombinedFlag { get; set; }

	[JsonPropertyName("api_deck_port")]
	public IEnumerable<ApiDeckPort>? ApiDeckPort { get; set; }

	[JsonPropertyName("api_dest_ship_slot")]
	public int ApiDestShipSlot { get; set; }

	[JsonPropertyName("api_event_object")]
	public ApiEventObject? ApiEventObject { get; set; }

	[JsonPropertyName("api_log")]
	public IEnumerable<ApiLog>? ApiLog { get; set; }

	[JsonPropertyName("api_material")]
	public IEnumerable<ApiMaterial>? ApiMaterial { get; set; }

	[JsonPropertyName("api_ndock")]
	public IEnumerable<ApiNdock>? ApiNdock { get; set; }

	[JsonPropertyName("api_p_bgm_id")]
	public int ApiPBgmId { get; set; }

	[JsonPropertyName("api_parallel_quest_count")]
	public int ApiParallelQuestCount { get; set; }

	[JsonPropertyName("api_plane_info")]
	public ApiPlaneInfo? ApiPlaneInfo { get; set; }

	[JsonPropertyName("api_ship")]
	public IEnumerable<ApiShip>? ApiShip { get; set; }

}
