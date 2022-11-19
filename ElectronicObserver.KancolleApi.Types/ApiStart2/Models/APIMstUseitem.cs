namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstUseitem
{
	[JsonPropertyName("api_category")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCategory { get; set; } = default!;

	[JsonPropertyName("api_description")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<string> ApiDescription { get; set; } = new();

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_price")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPrice { get; set; } = default!;

	[JsonPropertyName("api_usetype")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUsetype { get; set; } = default!;
}
