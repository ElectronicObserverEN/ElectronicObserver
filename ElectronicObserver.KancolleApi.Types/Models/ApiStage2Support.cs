namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage2Support
{
	[JsonPropertyName("api_f_count")]
	public int ApiFCount { get; set; }

	[JsonPropertyName("api_f_lostcount")]
	public int ApiFLostcount { get; set; }
}
