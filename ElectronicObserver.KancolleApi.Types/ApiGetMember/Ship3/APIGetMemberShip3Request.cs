namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship3;

public class ApiGetMemberShip3Request
{
	[JsonPropertyName("api_shipid")]
	public string ApiShipid { get; set; } = "";

	[JsonPropertyName("api_sort_key")]
	public string ApiSortKey { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("spi_sort_order")]
	public string SpiSortOrder { get; set; } = "";
}
