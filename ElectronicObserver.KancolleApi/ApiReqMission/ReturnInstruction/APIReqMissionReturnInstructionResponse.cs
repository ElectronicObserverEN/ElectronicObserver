namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.ReturnInstruction;

public class ApiReqMissionReturnInstructionResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_mission")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiMission { get; set; } = new();
}
