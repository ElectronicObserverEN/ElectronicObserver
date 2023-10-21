namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Start;

public class ApiReqMissionStartRequest
{
	[JsonPropertyName("api_deck_id")]
	public string ApiDeckId { get; set; } = "";

	[JsonPropertyName("api_mission")]
	public string ApiMission { get; set; } = "";

	[JsonPropertyName("api_mission_id")]
	public string ApiMissionId { get; set; } = "";

	[JsonPropertyName("api_serial_cid")]
	public string ApiSerialCid { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
