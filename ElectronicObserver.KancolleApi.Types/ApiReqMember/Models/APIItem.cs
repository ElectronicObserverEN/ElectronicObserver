namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiItem
{
	[JsonPropertyName("api_getmes")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiGetmes { get; set; } = default!;

	[JsonPropertyName("api_mode")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMode { get; set; } = default!;

	[JsonPropertyName("api_mst_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMstId { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiType { get; set; } = default!;
}
