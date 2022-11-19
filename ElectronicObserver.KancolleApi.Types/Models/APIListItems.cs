namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiListItems
{
	[JsonPropertyName("api_mission_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMissionId { get; set; } = default!;

	[JsonPropertyName("api_state")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiState { get; set; } = default!;
}
