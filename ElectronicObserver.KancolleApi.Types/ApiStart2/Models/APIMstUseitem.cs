namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstUseitem
{
	[JsonPropertyName("api_category")]
	public int ApiCategory { get; set; } = default!;

	[JsonPropertyName("api_description")]
	public List<string> ApiDescription { get; set; } = new();

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_price")]
	public int ApiPrice { get; set; } = default!;

	[JsonPropertyName("api_usetype")]
	public int ApiUsetype { get; set; } = default!;
}
