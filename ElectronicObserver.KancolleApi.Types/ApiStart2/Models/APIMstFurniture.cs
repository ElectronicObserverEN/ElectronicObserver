namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstFurniture
{
	[JsonPropertyName("api_active_flag")]
	public int ApiActiveFlag { get; set; }

	[JsonPropertyName("api_description")]
	public string ApiDescription { get; set; } = "";

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; }

	[JsonPropertyName("api_outside_id")]
	public int ApiOutsideId { get; set; }

	[JsonPropertyName("api_price")]
	public int ApiPrice { get; set; }

	[JsonPropertyName("api_rarity")]
	public int ApiRarity { get; set; }

	[JsonPropertyName("api_saleflg")]
	public int ApiSaleflg { get; set; }

	[JsonPropertyName("api_season")]
	public int ApiSeason { get; set; }

	[JsonPropertyName("api_title")]
	public string ApiTitle { get; set; } = "";

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; }

	[JsonPropertyName("api_version")]
	public int ApiVersion { get; set; }
}
