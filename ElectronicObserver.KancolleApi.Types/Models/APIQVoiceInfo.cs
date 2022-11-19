namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiqVoiceInfo
{
	[JsonPropertyName("api_icon_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiIconId { get; set; } = default!;

	[JsonPropertyName("api_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_voice_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiVoiceId { get; set; } = default!;

}
