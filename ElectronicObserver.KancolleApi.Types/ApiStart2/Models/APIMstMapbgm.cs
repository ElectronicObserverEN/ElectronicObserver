namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMapbgm
{
	[JsonPropertyName("api_boss_bgm")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiBossBgm { get; set; } = new();

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_map_bgm")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiMapBgm { get; set; } = new();

	[JsonPropertyName("api_maparea_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMapareaId { get; set; } = default!;

	[JsonPropertyName("api_moving_bgm")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMovingBgm { get; set; } = default!;

	[JsonPropertyName("api_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;
}
