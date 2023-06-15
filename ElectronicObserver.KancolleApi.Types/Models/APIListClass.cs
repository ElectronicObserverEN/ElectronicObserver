namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiListClass
{
	[JsonPropertyName("api_bonus_flag")]
	public int ApiBonusFlag { get; set; } = default!;

	[JsonPropertyName("api_category")]
	public int ApiCategory { get; set; } = default!;

	[JsonPropertyName("api_detail")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDetail { get; set; } = default!;

	[JsonPropertyName("api_get_material")]
	[Required]
	public List<int> ApiGetMaterial { get; set; } = new();

	[JsonPropertyName("api_invalid_flag")]
	public int ApiInvalidFlag { get; set; } = default!;

	[JsonPropertyName("api_lost_badges")]
	public int? ApiLostBadges { get; set; } = default!;

	[JsonPropertyName("api_no")]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_progress_flag")]
	public int ApiProgressFlag { get; set; } = default!;

	[JsonPropertyName("api_select_rewards")]
	public List<List<ApiSelectReward>>? ApiSelectRewards { get; set; } = default!;

	[JsonPropertyName("api_state")]
	public int ApiState { get; set; } = default!;

	[JsonPropertyName("api_title")]
	[Required(AllowEmptyStrings = true)]
	public string ApiTitle { get; set; } = default!;

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; } = default!;

	[JsonPropertyName("api_voice_id")]
	public int ApiVoiceId { get; set; } = default!;
}
