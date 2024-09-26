namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiPracticeShip
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_level")]
	public int? ApiLevel { get; set; }

	[JsonPropertyName("api_ship_id")]
	public int? ApiShipId { get; set; }

	[JsonPropertyName("api_star")]
	public int? ApiStar { get; set; }
}
