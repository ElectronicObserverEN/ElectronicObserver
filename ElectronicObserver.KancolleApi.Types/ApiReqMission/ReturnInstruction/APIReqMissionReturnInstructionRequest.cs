namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.ReturnInstruction;

public class ApiReqMissionReturnInstructionRequest
{
	[JsonPropertyName("api_deck_id")]
	public string ApiDeckId { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
