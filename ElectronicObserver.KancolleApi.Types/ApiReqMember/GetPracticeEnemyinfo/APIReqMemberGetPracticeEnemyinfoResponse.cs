

using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetPracticeEnemyinfo;

public class ApiReqMemberGetPracticeEnemyinfoResponse
{
	[JsonPropertyName("api_cmt")]
	public string ApiCmt { get; set; } = "";

	[JsonPropertyName("api_cmt_id")]
	public string ApiCmtId { get; set; } = "";

	[JsonPropertyName("api_deck")]
	public ApiPracticeDeck ApiDeck { get; set; } = new();

	[JsonPropertyName("api_deckname")]
	public string ApiDeckname { get; set; } = "";

	[JsonPropertyName("api_deckname_id")]
	public string ApiDecknameId { get; set; } = "";

	[JsonPropertyName("api_experience")]
	public List<int> ApiExperience { get; set; } = new();

	[JsonPropertyName("api_friend")]
	public int ApiFriend { get; set; }

	[JsonPropertyName("api_furniture")]
	public int ApiFurniture { get; set; }

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; }

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; }

	[JsonPropertyName("api_nickname")]
	public string ApiNickname { get; set; } = "";

	[JsonPropertyName("api_nickname_id")]
	public string ApiNicknameId { get; set; } = "";

	[JsonPropertyName("api_rank")]
	public int ApiRank { get; set; }

	[JsonPropertyName("api_ship")]
	public List<int> ApiShip { get; set; } = new();

	[JsonPropertyName("api_slotitem")]
	public List<int> ApiSlotitem { get; set; } = new();
}
