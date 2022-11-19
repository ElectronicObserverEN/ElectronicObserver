namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.ReturnInstruction;

public class ApiReqMissionReturnInstructionResponse
{
	[JsonPropertyName("api_mission")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<long> ApiMission { get; set; } = new();
}
