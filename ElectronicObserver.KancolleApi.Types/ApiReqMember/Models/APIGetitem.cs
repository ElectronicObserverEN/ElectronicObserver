

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiGetitem
{
	[JsonPropertyName("api_getcount")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiGetcount { get; set; } = default!;

	[JsonPropertyName("api_mst_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMstId { get; set; } = default!;

	[JsonPropertyName("api_slotitem")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiSlotitem? ApiSlotitem { get; set; } = default!;

	[JsonPropertyName("api_usemst")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUsemst { get; set; } = default!;
}
