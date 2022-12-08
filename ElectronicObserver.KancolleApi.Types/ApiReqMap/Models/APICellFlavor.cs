namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiCellFlavor
{
	[JsonPropertyName("api_message")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiMessage { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiType { get; set; } = default!;
}
