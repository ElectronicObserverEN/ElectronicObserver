namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage2
{
	[JsonPropertyName("api_air_fire")]
	public ApiAirFire? ApiAirFire { get; set; }

	[JsonPropertyName("api_e_count")]
	public int ApiECount { get; set; }

	[JsonPropertyName("api_e_lostcount")]
	public int ApiELostcount { get; set; }

	[JsonPropertyName("api_f_count")]
	public int ApiFCount { get; set; }

	[JsonPropertyName("api_f_lostcount")]
	public int ApiFLostcount { get; set; }
}
