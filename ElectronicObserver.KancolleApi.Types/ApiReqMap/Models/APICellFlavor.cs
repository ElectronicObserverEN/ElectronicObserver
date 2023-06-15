namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiCellFlavor
{
	[JsonPropertyName("api_message")]
	public string ApiMessage { get; set; } = "";

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; }
}
