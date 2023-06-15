namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Deck;

public class ApiGetMemberDeckRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
