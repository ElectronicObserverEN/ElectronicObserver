namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPortBasic
{
	[System.Text.Json.Serialization.JsonPropertyName("api_active_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiActiveFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_comment")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiComment { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_comment_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiCommentId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_count_deck")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCountDeck { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_count_kdock")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCountKdock { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_count_ndock")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCountNdock { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_experience")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiExperience { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_fcoin")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFcoin { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_firstflag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFirstflag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_fleetname")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public object? ApiFleetname { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_furniture")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFurniture { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_large_dock")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLargeDock { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_max_chara")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMaxChara { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_max_kagu")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMaxKagu { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_max_slotitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMaxSlotitem { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_medals")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMedals { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_member_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiMemberId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ms_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMsCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ms_success")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMsSuccess { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_nickname")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiNickname { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_nickname_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiNicknameId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_playtime")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiPlaytime { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_pt_challenged")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiPtChallenged { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_pt_challenged_win")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiPtChallengedWin { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_pt_lose")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiPtLose { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_pt_win")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiPtWin { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_pvp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiPvp { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_rank")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiRank { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_st_lose")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiStLose { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_st_win")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiStWin { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_starttime")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public long ApiStarttime { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_tutorial")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiTutorial { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_tutorial_progress")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiTutorialProgress { get; set; } = default!;
}
