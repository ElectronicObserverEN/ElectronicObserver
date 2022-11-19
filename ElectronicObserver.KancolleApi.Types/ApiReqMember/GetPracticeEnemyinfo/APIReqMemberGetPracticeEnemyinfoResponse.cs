

using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetPracticeEnemyinfo;

public class ApiReqMemberGetPracticeEnemyinfoResponse
{
	[JsonPropertyName("api_cmt")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiCmt { get; set; } = default!;

	[JsonPropertyName("api_cmt_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiCmtId { get; set; } = default!;

	[JsonPropertyName("api_deck")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiDeck ApiDeck { get; set; } = new();

	[JsonPropertyName("api_deckname")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckname { get; set; } = default!;

	[JsonPropertyName("api_deckname_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiDecknameId { get; set; } = default!;

	[JsonPropertyName("api_experience")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiExperience { get; set; } = new();

	[JsonPropertyName("api_friend")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFriend { get; set; } = default!;

	[JsonPropertyName("api_furniture")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFurniture { get; set; } = default!;

	[JsonPropertyName("api_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMemberId { get; set; } = default!;

	[JsonPropertyName("api_nickname")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiNickname { get; set; } = default!;

	[JsonPropertyName("api_nickname_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiNicknameId { get; set; } = default!;

	[JsonPropertyName("api_rank")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiRank { get; set; } = default!;

	[JsonPropertyName("api_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiShip { get; set; } = new();

	[JsonPropertyName("api_slotitem")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiSlotitem { get; set; } = new();
}
