namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage2
{
	[JsonPropertyName("api_f_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFCount { get; set; } = default!;

	[JsonPropertyName("api_f_lostcount")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFLostcount { get; set; } = default!;
}
