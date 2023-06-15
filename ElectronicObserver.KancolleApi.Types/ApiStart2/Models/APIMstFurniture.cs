namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstFurniture
{
	[JsonPropertyName("api_active_flag")]
	public int ApiActiveFlag { get; set; } = default!;

	[JsonPropertyName("api_description")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDescription { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_outside_id")]
	public int ApiOutsideId { get; set; } = default!;

	[JsonPropertyName("api_price")]
	public int ApiPrice { get; set; } = default!;

	[JsonPropertyName("api_rarity")]
	public int ApiRarity { get; set; } = default!;

	[JsonPropertyName("api_saleflg")]
	public int ApiSaleflg { get; set; } = default!;

	[JsonPropertyName("api_season")]
	public int ApiSeason { get; set; } = default!;

	[JsonPropertyName("api_title")]
	[Required(AllowEmptyStrings = true)]
	public string ApiTitle { get; set; } = default!;

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; } = default!;

	[JsonPropertyName("api_version")]
	public int ApiVersion { get; set; } = default!;
}
