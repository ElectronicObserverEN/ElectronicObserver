namespace ElectronicObserver.KancolleApi.Types.ApiReqFurniture.Change;

public class ApiReqFurnitureChangeRequest
{
	[JsonPropertyName("api_desk")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDesk { get; set; } = default!;

	[JsonPropertyName("api_floor")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiFloor { get; set; } = default!;

	[JsonPropertyName("api_season")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiSeason { get; set; } = default!;

	[JsonPropertyName("api_shelf")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiShelf { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_wallhanging")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiWallhanging { get; set; } = default!;

	[JsonPropertyName("api_wallpaper")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiWallpaper { get; set; } = default!;

	[JsonPropertyName("api_window")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiWindow { get; set; } = default!;
}
