namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;

public class ApiGetMemberShipDeckRequest
{
	[JsonPropertyName("api_deck_rid")]
	public string ApiDeckRid { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
