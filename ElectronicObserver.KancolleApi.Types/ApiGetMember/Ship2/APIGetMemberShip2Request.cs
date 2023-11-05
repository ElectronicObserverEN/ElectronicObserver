namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship2;

public class ApiGetMemberShip2Request
{
	[JsonPropertyName("api_sort_key")]
	public string ApiSortKey { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("spi_sort_order")]
	public string SpiSortOrder { get; set; } = "";
}
