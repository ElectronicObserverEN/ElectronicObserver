namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiCellFlavor
{
	[JsonPropertyName("api_message")]
	[Required(AllowEmptyStrings = true)]
	public string ApiMessage { get; set; } = default!;

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; } = default!;
}
