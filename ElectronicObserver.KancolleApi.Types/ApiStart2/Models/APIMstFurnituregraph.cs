namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstFurnituregraph
{
	[JsonPropertyName("api_filename")]
	public string ApiFilename { get; set; } = "";

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; }

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; }

	[JsonPropertyName("api_version")]
	public string ApiVersion { get; set; } = "";
}
