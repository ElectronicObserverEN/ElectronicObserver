namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Start;

public class ApiReqMissionStartResponse
{
	[JsonPropertyName("api_complatetime")]
	public long ApiComplatetime { get; set; } = default!;

	[JsonPropertyName("api_complatetime_str")]
	[Required(AllowEmptyStrings = true)]
	public string ApiComplatetimeStr { get; set; } = default!;
}
