using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiReqMap.Start.Response;

public class ApiEventmap
{

	[JsonPropertyName("api_dmg")]
	public int ApiDmg { get; set; }

	[JsonPropertyName("api_max_maphp")]
	public int ApiMaxMaphp { get; set; }

	[JsonPropertyName("api_now_maphp")]
	public int ApiNowMaphp { get; set; }

}