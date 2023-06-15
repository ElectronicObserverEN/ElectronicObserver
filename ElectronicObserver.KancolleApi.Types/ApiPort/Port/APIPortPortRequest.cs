namespace ElectronicObserver.KancolleApi.Types.ApiPort.Port;

public class ApiPortPortRequest
{
	[JsonPropertyName("api_port")]
	public string ApiPort { get; set; } = default!;

	[JsonPropertyName("api_sort_key")]
	public string ApiSortKey { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("spi_sort_order")]
	public string SpiSortOrder { get; set; } = default!;
}
