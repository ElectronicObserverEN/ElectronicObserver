namespace ElectronicObserver.KancolleApi.Types.ApiReqFurniture.Change;

public class ApiReqFurnitureChangeRequest
{
	[JsonPropertyName("api_desk")]
	public string ApiDesk { get; set; } = default!;

	[JsonPropertyName("api_floor")]
	public string ApiFloor { get; set; } = default!;

	[JsonPropertyName("api_season")]
	public string? ApiSeason { get; set; } = default!;

	[JsonPropertyName("api_shelf")]
	public string ApiShelf { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_wallhanging")]
	public string ApiWallhanging { get; set; } = default!;

	[JsonPropertyName("api_wallpaper")]
	public string ApiWallpaper { get; set; } = default!;

	[JsonPropertyName("api_window")]
	public string ApiWindow { get; set; } = default!;
}
