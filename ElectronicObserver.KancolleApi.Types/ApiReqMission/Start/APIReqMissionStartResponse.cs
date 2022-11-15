namespace ElectronicObserver.KancolleApi.Types.ApiReqMission.Start;

public class ApiReqMissionStartResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_complatetime")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiComplatetime { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_complatetime_str")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiComplatetimeStr { get; set; } = default!;
}
