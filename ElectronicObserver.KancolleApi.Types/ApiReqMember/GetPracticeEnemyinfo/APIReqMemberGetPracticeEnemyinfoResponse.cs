

using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetPracticeEnemyinfo;

public class ApiReqMemberGetPracticeEnemyinfoResponse
{
	[JsonPropertyName("api_cmt")]
	[Required(AllowEmptyStrings = true)]
	public string ApiCmt { get; set; } = default!;

	[JsonPropertyName("api_cmt_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiCmtId { get; set; } = default!;

	[JsonPropertyName("api_deck")]
	[Required]
	public ApiDeck ApiDeck { get; set; } = new();

	[JsonPropertyName("api_deckname")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDeckname { get; set; } = default!;

	[JsonPropertyName("api_deckname_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiDecknameId { get; set; } = default!;

	[JsonPropertyName("api_experience")]
	[Required]
	public List<int> ApiExperience { get; set; } = new();

	[JsonPropertyName("api_friend")]
	public int ApiFriend { get; set; } = default!;

	[JsonPropertyName("api_furniture")]
	public int ApiFurniture { get; set; } = default!;

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; } = default!;

	[JsonPropertyName("api_nickname")]
	[Required(AllowEmptyStrings = true)]
	public string ApiNickname { get; set; } = default!;

	[JsonPropertyName("api_nickname_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiNicknameId { get; set; } = default!;

	[JsonPropertyName("api_rank")]
	public int ApiRank { get; set; } = default!;

	[JsonPropertyName("api_ship")]
	[Required]
	public List<int> ApiShip { get; set; } = new();

	[JsonPropertyName("api_slotitem")]
	[Required]
	public List<int> ApiSlotitem { get; set; } = new();
}
