using ElectronicObserver.Core.Types;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage1
{
	[JsonPropertyName("api_disp_seiku")]
	public AirState ApiDispSeiku { get; set; }

	[JsonPropertyName("api_e_count")]
	public int ApiECount { get; set; }

	[JsonPropertyName("api_e_lostcount")]
	public int ApiELostcount { get; set; }

	[JsonPropertyName("api_f_count")]
	public int ApiFCount { get; set; }

	[JsonPropertyName("api_f_lostcount")]
	public int ApiFLostcount { get; set; }

	[JsonPropertyName("api_touch_plane")]
	public List<EquipmentId> ApiTouchPlane { get; set; } = new();
}
