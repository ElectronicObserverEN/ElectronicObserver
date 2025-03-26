namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiVolumeSetting
{
	[JsonPropertyName("api_be_left")]
	public int ApiBeLeft { get; set; }

	[JsonPropertyName("api_duty")]
	public int ApiDuty { get; set; }

	[JsonPropertyName("api_bgm")]
	public int ApiBgm { get; set; }

	[JsonPropertyName("api_se")]
	public int ApiSe { get; set; }

	[JsonPropertyName("api_voice")]
	public int ApiVoice { get; set; }
}
