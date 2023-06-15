namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPortBasic
{
	[JsonPropertyName("api_active_flag")]
	public int ApiActiveFlag { get; set; } = default!;

	[JsonPropertyName("api_comment")]
	public string ApiComment { get; set; } = default!;

	[JsonPropertyName("api_comment_id")]
	public string ApiCommentId { get; set; } = default!;

	[JsonPropertyName("api_count_deck")]
	public int ApiCountDeck { get; set; } = default!;

	[JsonPropertyName("api_count_kdock")]
	public int ApiCountKdock { get; set; } = default!;

	[JsonPropertyName("api_count_ndock")]
	public int ApiCountNdock { get; set; } = default!;

	[JsonPropertyName("api_experience")]
	public int ApiExperience { get; set; } = default!;

	[JsonPropertyName("api_fcoin")]
	public int ApiFcoin { get; set; } = default!;

	[JsonPropertyName("api_firstflag")]
	public int ApiFirstflag { get; set; } = default!;

	[JsonPropertyName("api_fleetname")]
	public object? ApiFleetname { get; set; } = default!;

	[JsonPropertyName("api_furniture")]
	public List<int> ApiFurniture { get; set; } = new();

	[JsonPropertyName("api_large_dock")]
	public int ApiLargeDock { get; set; } = default!;

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_max_chara")]
	public int ApiMaxChara { get; set; } = default!;

	[JsonPropertyName("api_max_kagu")]
	public int ApiMaxKagu { get; set; } = default!;

	[JsonPropertyName("api_max_slotitem")]
	public int ApiMaxSlotitem { get; set; } = default!;

	[JsonPropertyName("api_medals")]
	public int ApiMedals { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	public string ApiMemberId { get; set; } = default!;

	[JsonPropertyName("api_ms_count")]
	public int ApiMsCount { get; set; } = default!;

	[JsonPropertyName("api_ms_success")]
	public int ApiMsSuccess { get; set; } = default!;

	[JsonPropertyName("api_nickname")]
	public string ApiNickname { get; set; } = default!;

	[JsonPropertyName("api_nickname_id")]
	public string ApiNicknameId { get; set; } = default!;

	[JsonPropertyName("api_playtime")]
	public int ApiPlaytime { get; set; } = default!;

	[JsonPropertyName("api_pt_challenged")]
	public int ApiPtChallenged { get; set; } = default!;

	[JsonPropertyName("api_pt_challenged_win")]
	public int ApiPtChallengedWin { get; set; } = default!;

	[JsonPropertyName("api_pt_lose")]
	public int ApiPtLose { get; set; } = default!;

	[JsonPropertyName("api_pt_win")]
	public int ApiPtWin { get; set; } = default!;

	[JsonPropertyName("api_pvp")]
	public List<int> ApiPvp { get; set; } = new();

	[JsonPropertyName("api_rank")]
	public int ApiRank { get; set; } = default!;

	[JsonPropertyName("api_st_lose")]
	public int ApiStLose { get; set; } = default!;

	[JsonPropertyName("api_st_win")]
	public int ApiStWin { get; set; } = default!;

	[JsonPropertyName("api_starttime")]
	public long ApiStarttime { get; set; } = default!;

	[JsonPropertyName("api_tutorial")]
	public int ApiTutorial { get; set; } = default!;

	[JsonPropertyName("api_tutorial_progress")]
	public int ApiTutorialProgress { get; set; } = default!;
}
