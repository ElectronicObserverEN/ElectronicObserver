namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship2;

public class ApiGetMemberShip2Request
{
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
