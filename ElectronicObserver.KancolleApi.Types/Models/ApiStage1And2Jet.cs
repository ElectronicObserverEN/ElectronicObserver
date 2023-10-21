namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage1And2Jet
{
	[JsonPropertyName("api_e_count")]
	public int ApiECount { get; set; }

	[JsonPropertyName("api_e_lostcount")]
	public int ApiELostcount { get; set; }

	[JsonPropertyName("api_f_count")]
	public int ApiFCount { get; set; }

	[JsonPropertyName("api_f_lostcount")]
	public int ApiFLostcount { get; set; }
}
