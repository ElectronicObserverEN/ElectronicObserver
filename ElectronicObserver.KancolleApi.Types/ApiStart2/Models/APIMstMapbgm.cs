namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMapbgm
{
	[JsonPropertyName("api_boss_bgm")]
	public List<int> ApiBossBgm { get; set; } = new();

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_map_bgm")]
	public List<int> ApiMapBgm { get; set; } = new();

	[JsonPropertyName("api_maparea_id")]
	public int ApiMapareaId { get; set; }

	[JsonPropertyName("api_moving_bgm")]
	public int ApiMovingBgm { get; set; }

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; }
}
