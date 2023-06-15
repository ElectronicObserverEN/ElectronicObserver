namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiShip
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_level")]
	public int? ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	public int? ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_star")]
	public int? ApiStar { get; set; } = default!;
}
