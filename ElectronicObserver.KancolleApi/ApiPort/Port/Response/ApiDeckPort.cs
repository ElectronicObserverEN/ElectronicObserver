using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiPort.Port.Response;

public class ApiDeckPort
{

	[JsonPropertyName("api_flagship")]
	public string? ApiFlagship { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; }

	[JsonPropertyName("api_mission")]
	public IEnumerable<decimal>? ApiMission { get; set; }

	[JsonPropertyName("api_name")]
	public string? ApiName { get; set; }

	[JsonPropertyName("api_name_id")]
	public string? ApiNameId { get; set; }

	[JsonPropertyName("api_ship")]
	public IEnumerable<int>? ApiShip { get; set; }

}
