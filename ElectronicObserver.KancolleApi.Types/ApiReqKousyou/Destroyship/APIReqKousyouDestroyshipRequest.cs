namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyship;

public class ApiReqKousyouDestroyshipRequest
{
	[JsonPropertyName("api_ship_id")]
	public string ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_slot_dest_flag")]
	public string ApiSlotDestFlag { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
