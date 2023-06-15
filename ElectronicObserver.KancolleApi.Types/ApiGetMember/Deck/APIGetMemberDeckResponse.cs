﻿namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Deck;

public class ApiGetMemberDeckResponse
{
	[JsonPropertyName("api_flagship")]
	public string ApiFlagship { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; }

	[JsonPropertyName("api_mission")]
	public List<long> ApiMission { get; set; } = new();

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; }

	[JsonPropertyName("api_name_id")]
	public string ApiNameId { get; set; }

	[JsonPropertyName("api_ship")]
	public List<int> ApiShip { get; set; } = new();
}
