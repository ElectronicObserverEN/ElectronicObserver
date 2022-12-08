namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiListClass
{
	[JsonPropertyName("api_bonus_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBonusFlag { get; set; } = default!;

	[JsonPropertyName("api_category")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCategory { get; set; } = default!;

	[JsonPropertyName("api_detail")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDetail { get; set; } = default!;

	[JsonPropertyName("api_get_material")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiGetMaterial { get; set; } = new();

	[JsonPropertyName("api_invalid_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiInvalidFlag { get; set; } = default!;

	[JsonPropertyName("api_lost_badges")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiLostBadges { get; set; } = default!;

	[JsonPropertyName("api_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_progress_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiProgressFlag { get; set; } = default!;

	[JsonPropertyName("api_select_rewards")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<List<ApiSelectReward>>? ApiSelectRewards { get; set; } = default!;

	[JsonPropertyName("api_state")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiState { get; set; } = default!;

	[JsonPropertyName("api_title")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiTitle { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiType { get; set; } = default!;

	[JsonPropertyName("api_voice_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiVoiceId { get; set; } = default!;
}
