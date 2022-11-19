namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstStype
{
	[JsonPropertyName("api_equip_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public IDictionary<string, int> ApiEquipType { get; set; } = new Dictionary<string, int>();

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_kcnt")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiKcnt { get; set; } = default!;

	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_scnt")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiScnt { get; set; } = default!;

	[JsonPropertyName("api_sortno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSortno { get; set; } = default!;
}
