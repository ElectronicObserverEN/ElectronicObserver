namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Start;

public class ApiReqMissionStartRequest
{
	[JsonPropertyName("api_deck_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_mission")]
	[Required(AllowEmptyStrings = true)]
	public string ApiMission { get; set; } = default!;

	[JsonPropertyName("api_mission_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiMissionId { get; set; } = default!;

	[JsonPropertyName("api_serial_cid")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSerialCid { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
