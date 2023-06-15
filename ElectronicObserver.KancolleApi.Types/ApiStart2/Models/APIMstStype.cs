namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstStype
{
	[JsonPropertyName("api_equip_type")]
	public IDictionary<string, int> ApiEquipType { get; set; } = new Dictionary<string, int>();

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_kcnt")]
	public int ApiKcnt { get; set; }

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_scnt")]
	public int ApiScnt { get; set; }

	[JsonPropertyName("api_sortno")]
	public int ApiSortno { get; set; }
}
