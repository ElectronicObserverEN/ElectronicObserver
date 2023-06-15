namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.ReturnInstruction;

public class ApiReqMissionReturnInstructionResponse
{
	[JsonPropertyName("api_mission")]
	[Required]
	public List<long> ApiMission { get; set; } = new();
}
