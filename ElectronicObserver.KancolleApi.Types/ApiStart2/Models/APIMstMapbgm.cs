namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMapbgm
{
	[JsonPropertyName("api_boss_bgm")]
	[Required]
	public List<int> ApiBossBgm { get; set; } = new();

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_map_bgm")]
	[Required]
	public List<int> ApiMapBgm { get; set; } = new();

	[JsonPropertyName("api_maparea_id")]
	public int ApiMapareaId { get; set; } = default!;

	[JsonPropertyName("api_moving_bgm")]
	public int ApiMovingBgm { get; set; } = default!;

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; } = default!;
}
