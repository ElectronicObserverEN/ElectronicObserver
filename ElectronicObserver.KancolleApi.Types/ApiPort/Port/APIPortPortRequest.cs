namespace ElectronicObserver.KancolleApi.Types.ApiPort.Port;

public class ApiPortPortRequest
{
	[JsonPropertyName("api_port")]
	[Required(AllowEmptyStrings = true)]
	public string ApiPort { get; set; } = default!;

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
