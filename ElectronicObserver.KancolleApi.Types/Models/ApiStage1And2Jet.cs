namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage1And2Jet
{
	[JsonPropertyName("api_e_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiECount { get; set; }

	[JsonPropertyName("api_e_lostcount")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiELostcount { get; set; }

	[JsonPropertyName("api_f_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFCount { get; set; }

	[JsonPropertyName("api_f_lostcount")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFLostcount { get; set; }
}
