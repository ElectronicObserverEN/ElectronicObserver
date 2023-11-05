namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.PresetSlot;

public class ApiGetMemberPresetSlotRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
