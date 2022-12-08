namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.PresetDeck;

public class ApiGetMemberPresetDeckRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
