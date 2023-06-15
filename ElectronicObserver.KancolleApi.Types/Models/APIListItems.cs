namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiListItems
{
	[JsonPropertyName("api_mission_id")]
	public int ApiMissionId { get; set; } = default!;

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; } = default!;
}
