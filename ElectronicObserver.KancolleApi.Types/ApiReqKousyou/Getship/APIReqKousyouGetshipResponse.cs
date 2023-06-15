using ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Getship;

public class ApiReqKousyouGetshipResponse
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_kdock")]
	[Required]
	public List<ApiGetMemberKdockResponse> ApiKdock { get; set; } = new();

	[JsonPropertyName("api_ship")]
	[Required]
	public ApiShip ApiShip { get; set; } = new();

	[JsonPropertyName("api_ship_id")]
	public int ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_slotitem")]
	[Required]
	public List<ApiSlotitem> ApiSlotitem { get; set; } = new();
}
