namespace ElectronicObserver.KancolleApi.Types.ApiReqFurniture.Change;

public class ApiReqFurnitureChangeRequest
{
	[JsonPropertyName("api_desk")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDesk { get; set; } = default!;

	[JsonPropertyName("api_floor")]
	[Required(AllowEmptyStrings = true)]
	public string ApiFloor { get; set; } = default!;

	[JsonPropertyName("api_season")]
	public string? ApiSeason { get; set; } = default!;

	[JsonPropertyName("api_shelf")]
	[Required(AllowEmptyStrings = true)]
	public string ApiShelf { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_wallhanging")]
	[Required(AllowEmptyStrings = true)]
	public string ApiWallhanging { get; set; } = default!;

	[JsonPropertyName("api_wallpaper")]
	[Required(AllowEmptyStrings = true)]
	public string ApiWallpaper { get; set; } = default!;

	[JsonPropertyName("api_window")]
	[Required(AllowEmptyStrings = true)]
	public string ApiWindow { get; set; } = default!;
}
