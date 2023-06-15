using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage1
{
	[JsonPropertyName("api_disp_seiku")]
	public AirState ApiDispSeiku { get; set; } = default!;

	[JsonPropertyName("api_e_count")]
	public int ApiECount { get; set; } = default!;

	[JsonPropertyName("api_e_lostcount")]
	public int ApiELostcount { get; set; } = default!;

	[JsonPropertyName("api_f_count")]
	public int ApiFCount { get; set; } = default!;

	[JsonPropertyName("api_f_lostcount")]
	public int ApiFLostcount { get; set; } = default!;

	[JsonPropertyName("api_touch_plane")]
	[Required]
	public List<EquipmentId> ApiTouchPlane { get; set; } = new();
}
