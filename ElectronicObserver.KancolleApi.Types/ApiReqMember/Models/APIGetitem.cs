namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiGetitem
{
	[JsonPropertyName("api_getcount")]
	public int ApiGetcount { get; set; }

	[JsonPropertyName("api_mst_id")]
	public int ApiMstId { get; set; }

	/// <summary>
	/// Element type is <see cref="Models.ApiSlotitem"/> or <see cref="List{T}"/> of <see cref="Models.ApiSlotitem"/>s.
	/// </summary>
	[JsonPropertyName("api_slotitem")]
	public object? ApiSlotitem { get; set; }

	[JsonPropertyName("api_usemst")]
	public int ApiUsemst { get; set; }
}
