namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiListClass
{
	[JsonPropertyName("api_bonus_flag")]
	public int ApiBonusFlag { get; set; }

	[JsonPropertyName("api_category")]
	public int ApiCategory { get; set; }

	[JsonPropertyName("api_detail")]
	public string ApiDetail { get; set; } = "";

	[JsonPropertyName("api_get_material")]
	public List<int> ApiGetMaterial { get; set; } = new();

	[JsonPropertyName("api_invalid_flag")]
	public int ApiInvalidFlag { get; set; }

	[JsonPropertyName("api_lost_badges")]
	public int? ApiLostBadges { get; set; }

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; }

	[JsonPropertyName("api_progress_flag")]
	public int ApiProgressFlag { get; set; }

	[JsonPropertyName("api_select_rewards")]
	public List<List<ApiSelectReward>>? ApiSelectRewards { get; set; }

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; }

	[JsonPropertyName("api_title")]
	public string ApiTitle { get; set; } = "";

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; }

	[JsonPropertyName("api_voice_id")]
	public int ApiVoiceId { get; set; }
}
