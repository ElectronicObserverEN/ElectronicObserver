namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiGetUseitem
{
	[JsonPropertyName("api_useitem_id")]
	public int ApiUseitemId { get; set; } = default!;

	[JsonPropertyName("api_useitem_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiUseitemName { get; set; } = default!;
}
