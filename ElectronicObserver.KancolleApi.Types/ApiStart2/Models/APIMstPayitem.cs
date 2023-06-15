namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstPayitem
{
	[JsonPropertyName("api_description")]
	public string ApiDescription { get; set; } = "";

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_item")]
	public List<int> ApiItem { get; set; } = new();

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_price")]
	public int ApiPrice { get; set; }

	[JsonPropertyName("api_shop_description")]
	public string ApiShopDescription { get; set; } = "";

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; }
}
