namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiqVoiceInfo
{
	[JsonPropertyName("api_icon_id")]
	public int ApiIconId { get; set; } = default!;

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_voice_id")]
	public int ApiVoiceId { get; set; } = default!;

}
