namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship3;

public class ApiGetMemberShip3Request
{
	[JsonPropertyName("api_shipid")]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipid { get; set; } = default!;

	[JsonPropertyName("api_sort_key")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSortKey { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("spi_sort_order")]
	[Required(AllowEmptyStrings = true)]
	public string SpiSortOrder { get; set; } = default!;
}
