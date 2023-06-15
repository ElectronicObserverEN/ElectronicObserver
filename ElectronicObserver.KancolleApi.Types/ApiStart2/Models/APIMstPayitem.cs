namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstPayitem
{
	[JsonPropertyName("api_description")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDescription { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_item")]
	[Required]
	public List<int> ApiItem { get; set; } = new();

	[JsonPropertyName("api_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_price")]
	public int ApiPrice { get; set; } = default!;

	[JsonPropertyName("api_shop_description")]
	[Required(AllowEmptyStrings = true)]
	public string ApiShopDescription { get; set; } = default!;

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; } = default!;
}
