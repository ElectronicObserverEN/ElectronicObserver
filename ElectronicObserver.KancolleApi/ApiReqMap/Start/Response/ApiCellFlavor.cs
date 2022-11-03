using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiReqMap.Start.Response;

public class ApiCellFlavor
{

	[JsonPropertyName("api_message")]
	public string ApiMessage { get; set; }

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; }

}