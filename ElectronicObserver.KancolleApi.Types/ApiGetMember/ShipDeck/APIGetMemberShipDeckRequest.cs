namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;

public class ApiGetMemberShipDeckRequest
{
	[JsonPropertyName("api_deck_rid")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckRid { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
