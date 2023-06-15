using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship3;

public class ApiGetMemberShip3Response
{
	[JsonPropertyName("api_deck_data")]
	[Required]
	public List<ApiDeckDatum> ApiDeckData { get; set; } = new();

	[JsonPropertyName("api_ship_data")]
	[Required]
	public List<ApiShipDatum> ApiShipData { get; set; } = new();

	[JsonPropertyName("api_slot_data")]
	[Required]
	public IDictionary<string, List<int>> ApiSlotData { get; set; } = new Dictionary<string, List<int>>();
}
