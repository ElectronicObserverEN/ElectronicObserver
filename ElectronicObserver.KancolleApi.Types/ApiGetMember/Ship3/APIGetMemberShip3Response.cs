using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship3;

public class ApiGetMemberShip3Response
{
	[JsonPropertyName("api_deck_data")]
	public List<FleetDataDto> ApiDeckData { get; set; } = new();

	[JsonPropertyName("api_ship_data")]
	public List<ApiShip> ApiShipData { get; set; } = new();

	[JsonPropertyName("api_slot_data")]
	public IDictionary<string, List<int>> ApiSlotData { get; set; } = new Dictionary<string, List<int>>();
}
