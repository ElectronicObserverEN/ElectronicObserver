namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstFurniture
{
	[JsonPropertyName("api_active_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiActiveFlag { get; set; } = default!;

	[JsonPropertyName("api_description")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDescription { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_outside_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiOutsideId { get; set; } = default!;

	[JsonPropertyName("api_price")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPrice { get; set; } = default!;

	[JsonPropertyName("api_rarity")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRarity { get; set; } = default!;

	[JsonPropertyName("api_saleflg")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSaleflg { get; set; } = default!;

	[JsonPropertyName("api_season")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSeason { get; set; } = default!;

	[JsonPropertyName("api_title")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiTitle { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiType { get; set; } = default!;

	[JsonPropertyName("api_version")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiVersion { get; set; } = default!;
}
