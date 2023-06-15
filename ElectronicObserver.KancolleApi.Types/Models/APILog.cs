namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiLog
{
	[JsonPropertyName("api_message")]
	[Required(AllowEmptyStrings = true)]
	public string ApiMessage { get; set; } = default!;

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_state")]
	[Required(AllowEmptyStrings = true)]
	public string ApiState { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[Required(AllowEmptyStrings = true)]
	public string ApiType { get; set; } = default!;
}
