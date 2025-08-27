namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFriendlySetting
{
	[JsonPropertyName("api_request_flag")]
	public FriendFleetRequestFlag ApiRequestFlag { get; set; }

	[JsonPropertyName("api_request_type")]
	public FriendFleetRequestType ApiRequestType { get; set; }
}
