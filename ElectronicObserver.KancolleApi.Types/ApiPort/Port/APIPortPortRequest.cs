namespace ElectronicObserver.KancolleApi.Types.ApiPort.Port;

public class ApiPortPortRequest
{
	[JsonPropertyName("api_port")]
	public string ApiPort { get; set; } = "";

	[JsonPropertyName("api_sort_key")]
	public string ApiSortKey { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("spi_sort_order")]
	public string SpiSortOrder { get; set; } = "";
}
