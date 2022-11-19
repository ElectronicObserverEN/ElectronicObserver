namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPortBasic
{
	[JsonPropertyName("api_active_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiActiveFlag { get; set; } = default!;

	[JsonPropertyName("api_comment")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiComment { get; set; } = default!;

	[JsonPropertyName("api_comment_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiCommentId { get; set; } = default!;

	[JsonPropertyName("api_count_deck")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCountDeck { get; set; } = default!;

	[JsonPropertyName("api_count_kdock")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCountKdock { get; set; } = default!;

	[JsonPropertyName("api_count_ndock")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCountNdock { get; set; } = default!;

	[JsonPropertyName("api_experience")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiExperience { get; set; } = default!;

	[JsonPropertyName("api_fcoin")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFcoin { get; set; } = default!;

	[JsonPropertyName("api_firstflag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFirstflag { get; set; } = default!;

	[JsonPropertyName("api_fleetname")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public object? ApiFleetname { get; set; } = default!;

	[JsonPropertyName("api_furniture")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFurniture { get; set; } = new();

	[JsonPropertyName("api_large_dock")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLargeDock { get; set; } = default!;

	[JsonPropertyName("api_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_max_chara")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMaxChara { get; set; } = default!;

	[JsonPropertyName("api_max_kagu")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMaxKagu { get; set; } = default!;

	[JsonPropertyName("api_max_slotitem")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMaxSlotitem { get; set; } = default!;

	[JsonPropertyName("api_medals")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMedals { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiMemberId { get; set; } = default!;

	[JsonPropertyName("api_ms_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMsCount { get; set; } = default!;

	[JsonPropertyName("api_ms_success")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMsSuccess { get; set; } = default!;

	[JsonPropertyName("api_nickname")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiNickname { get; set; } = default!;

	[JsonPropertyName("api_nickname_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiNicknameId { get; set; } = default!;

	[JsonPropertyName("api_playtime")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPlaytime { get; set; } = default!;

	[JsonPropertyName("api_pt_challenged")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPtChallenged { get; set; } = default!;

	[JsonPropertyName("api_pt_challenged_win")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPtChallengedWin { get; set; } = default!;

	[JsonPropertyName("api_pt_lose")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPtLose { get; set; } = default!;

	[JsonPropertyName("api_pt_win")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiPtWin { get; set; } = default!;

	[JsonPropertyName("api_pvp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiPvp { get; set; } = new();

	[JsonPropertyName("api_rank")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRank { get; set; } = default!;

	[JsonPropertyName("api_st_lose")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiStLose { get; set; } = default!;

	[JsonPropertyName("api_st_win")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiStWin { get; set; } = default!;

	[JsonPropertyName("api_starttime")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public long ApiStarttime { get; set; } = default!;

	[JsonPropertyName("api_tutorial")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiTutorial { get; set; } = default!;

	[JsonPropertyName("api_tutorial_progress")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiTutorialProgress { get; set; } = default!;
}
