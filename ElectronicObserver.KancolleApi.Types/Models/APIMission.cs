namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiMission
{
	[JsonPropertyName("api_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiCount { get; set; } = default!;

	[JsonPropertyName("api_rate")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiRate { get; set; } = default!;

	[JsonPropertyName("api_success")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiSuccess { get; set; } = default!;
}
