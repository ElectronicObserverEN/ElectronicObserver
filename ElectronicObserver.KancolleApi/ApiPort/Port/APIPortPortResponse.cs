using ElectronicObserver.KancolleApi.Types.ApiGetMember.Ndock;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiPort.Port;

public class ApiPortPortResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_basic")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiPortBasic ApiBasic { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_c_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiCFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_combined_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiCombinedFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_deck_port")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiDeckPort> ApiDeckPort { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_dest_ship_slot")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiDestShipSlot { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_event_object")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiEventObject? ApiEventObject { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_log")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiLog> ApiLog { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_material")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMaterial> ApiMaterial { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ndock")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiGetMemberNdockResponse> ApiNdock { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_p_bgm_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiPBgmId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_parallel_quest_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiParallelQuestCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_plane_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiPortPlaneInfo? ApiPlaneInfo { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiShip> ApiShip { get; set; } = new();
}
