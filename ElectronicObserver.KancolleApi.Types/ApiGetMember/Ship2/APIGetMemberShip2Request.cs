namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship2;

public class ApiGetMemberShip2Request
{
	[JsonPropertyName("api_token")]
	public required string ApiToken { get; set; }

	[JsonPropertyName("api_verno")]
	public required string ApiVerno { get; set; }

	[JsonPropertyName("api_sort_key")]
	public required string ApiSortKey { get; set; }

	[JsonPropertyName("spi_sort_order")]
	public required string SpiSortOrder { get; set; }
}
