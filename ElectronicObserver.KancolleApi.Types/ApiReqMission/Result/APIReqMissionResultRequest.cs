namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;

public class ApiReqMissionResultRequest
{
	[JsonPropertyName("api_deck_id")]
	public string ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
