using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;

public class ApiGetMemberShipDeckResponse
{
	[JsonPropertyName("api_deck_data")]
	public List<ApiDeckDatum> ApiDeckData { get; set; } = new();

	[JsonPropertyName("api_ship_data")]
	public List<ApiShipDatum> ApiShipData { get; set; } = new();
}
