namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.PresetDeck;

public class ApiGetMemberPresetDeckRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
