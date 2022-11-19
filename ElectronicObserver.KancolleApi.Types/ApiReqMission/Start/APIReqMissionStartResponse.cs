namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Start;

public class ApiReqMissionStartResponse
{
	[JsonPropertyName("api_complatetime")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public long ApiComplatetime { get; set; } = default!;

	[JsonPropertyName("api_complatetime_str")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiComplatetimeStr { get; set; } = default!;
}
