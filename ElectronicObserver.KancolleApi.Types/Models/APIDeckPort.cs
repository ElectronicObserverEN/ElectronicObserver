namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiDeckPort
{
	[JsonPropertyName("api_flagship")]
	public string ApiFlagship { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; } = default!;

	[JsonPropertyName("api_mission")]
	public List<long> ApiMission { get; set; } = new();

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_name_id")]
	public string ApiNameId { get; set; } = default!;

	[JsonPropertyName("api_ship")]
	public List<int> ApiShip { get; set; } = new();
}
