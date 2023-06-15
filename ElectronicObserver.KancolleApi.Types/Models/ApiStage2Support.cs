namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage2Support
{
	[JsonPropertyName("api_f_count")]
	public int ApiFCount { get; set; } = default!;

	[JsonPropertyName("api_f_lostcount")]
	public int ApiFLostcount { get; set; } = default!;
}
