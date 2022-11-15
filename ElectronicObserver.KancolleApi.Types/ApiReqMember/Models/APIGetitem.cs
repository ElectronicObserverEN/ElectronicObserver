

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiGetitem
{
	[System.Text.Json.Serialization.JsonPropertyName("api_getcount")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiGetcount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMstId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_slotitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public ApiSlotitem? ApiSlotitem { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_usemst")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiUsemst { get; set; } = default!;
}
