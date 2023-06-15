namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship3;

public class ApiGetMemberShip3Request
{
	[JsonPropertyName("api_shipid")]
	public string ApiShipid { get; set; } = default!;

	[JsonPropertyName("api_sort_key")]
	public string ApiSortKey { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("spi_sort_order")]
	public string SpiSortOrder { get; set; } = default!;
}
