namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Deck;

public class ApiGetMemberDeckRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
