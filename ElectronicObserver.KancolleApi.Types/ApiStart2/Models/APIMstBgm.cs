namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstBgm
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = default!;
}
