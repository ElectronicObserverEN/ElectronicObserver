namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiShip
{
	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_star")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiStar { get; set; } = default!;
}
