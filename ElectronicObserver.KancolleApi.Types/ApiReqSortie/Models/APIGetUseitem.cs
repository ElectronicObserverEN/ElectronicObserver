namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiGetUseitem
{
	[JsonPropertyName("api_useitem_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUseitemId { get; set; } = default!;

	[JsonPropertyName("api_useitem_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiUseitemName { get; set; } = default!;
}
