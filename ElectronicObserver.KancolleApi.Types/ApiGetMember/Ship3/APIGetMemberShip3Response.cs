using ElectronicObserver.KancolleApi.Types.ApiGetMember.Unsetslot;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship3;

public class ApiGetMemberShip3Response
{
	[JsonPropertyName("api_deck_data")]
	public List<FleetDataDto> ApiDeckData { get; set; } = [];

	[JsonPropertyName("api_ship_data")]
	public List<ApiShip> ApiShipData { get; set; } = [];

	[JsonPropertyName("api_slot_data")]
	public ApiGetMemberUnsetslotResponse ApiSlotData { get; set; } = [];
}
