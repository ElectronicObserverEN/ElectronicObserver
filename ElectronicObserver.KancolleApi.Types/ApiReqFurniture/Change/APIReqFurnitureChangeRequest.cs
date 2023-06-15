namespace ElectronicObserver.KancolleApi.Types.ApiReqFurniture.Change;

public class ApiReqFurnitureChangeRequest
{
	[JsonPropertyName("api_desk")]
	public string ApiDesk { get; set; } = "";

	[JsonPropertyName("api_floor")]
	public string ApiFloor { get; set; } = "";

	[JsonPropertyName("api_season")]
	public string? ApiSeason { get; set; }

	[JsonPropertyName("api_shelf")]
	public string ApiShelf { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("api_wallhanging")]
	public string ApiWallhanging { get; set; } = "";

	[JsonPropertyName("api_wallpaper")]
	public string ApiWallpaper { get; set; } = "";

	[JsonPropertyName("api_window")]
	public string ApiWindow { get; set; } = "";
}
