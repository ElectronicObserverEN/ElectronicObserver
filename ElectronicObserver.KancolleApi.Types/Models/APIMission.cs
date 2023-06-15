namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiMission
{
	[JsonPropertyName("api_count")]
	[Required(AllowEmptyStrings = true)]
	public string ApiCount { get; set; } = default!;

	[JsonPropertyName("api_rate")]
	[Required(AllowEmptyStrings = true)]
	public string ApiRate { get; set; } = default!;

	[JsonPropertyName("api_success")]
	[Required(AllowEmptyStrings = true)]
	public string ApiSuccess { get; set; } = default!;
}
