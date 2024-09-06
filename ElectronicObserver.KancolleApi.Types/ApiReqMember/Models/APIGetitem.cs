namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiGetitem
{
	[JsonPropertyName("api_getcount")]
	public int ApiGetcount { get; set; }

	[JsonPropertyName("api_mst_id")]
	public int ApiMstId { get; set; }

	[JsonPropertyName("api_slotitem")]
	public ApiSlotitem? ApiSlotitem { get; set; }

	[JsonPropertyName("api_usemst")]
	public int ApiUsemst { get; set; }
}
