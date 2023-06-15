namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstFurnituregraph
{
	[JsonPropertyName("api_filename")]
	[Required(AllowEmptyStrings = true)]
	public string ApiFilename { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; } = default!;

	[JsonPropertyName("api_version")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVersion { get; set; } = default!;
}
