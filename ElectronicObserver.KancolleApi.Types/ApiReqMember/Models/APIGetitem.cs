namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiGetitem
{
	[JsonPropertyName("api_getcount")]
	public int ApiGetcount { get; set; }

	[JsonPropertyName("api_mst_id")]
	public int ApiMstId { get; set; }

	/// <summary>
	/// Element type is <see cref="Models.ApiSlotitem"/> or <see cref="List{T}"/> of <see cref="Models.ApiSlotitem"/>s.
	/// It seems like the only instance of this being a list was when exchanging teru teru bozu for an antenna (equipment id 532), 2024/06/28 00:17:04 JST
	/// </summary>
	[JsonPropertyName("api_slotitem")]
	public object? ApiSlotitem { get; set; }

	[JsonPropertyName("api_usemst")]
	public int ApiUsemst { get; set; }
}
