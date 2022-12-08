namespace ElectronicObserver.KancolleApi.Types.ApiPort.Port;

public class ApiPortPortRequest
{
	[JsonPropertyName("api_port")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiPort { get; set; } = default!;

	[JsonPropertyName("api_sort_key")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiSortKey { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("spi_sort_order")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string SpiSortOrder { get; set; } = default!;
}
