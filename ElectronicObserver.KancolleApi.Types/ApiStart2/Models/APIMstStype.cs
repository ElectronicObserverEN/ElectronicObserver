namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstStype
{
	[JsonPropertyName("api_equip_type")]
	[Required]
	public IDictionary<string, int> ApiEquipType { get; set; } = new Dictionary<string, int>();

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_kcnt")]
	public int ApiKcnt { get; set; } = default!;

	[JsonPropertyName("api_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_scnt")]
	public int ApiScnt { get; set; } = default!;

	[JsonPropertyName("api_sortno")]
	public int ApiSortno { get; set; } = default!;
}
