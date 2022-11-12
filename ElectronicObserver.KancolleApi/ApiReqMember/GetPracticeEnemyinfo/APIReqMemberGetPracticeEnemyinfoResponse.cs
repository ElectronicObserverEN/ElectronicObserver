

using ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.GetPracticeEnemyinfo;

public class ApiReqMemberGetPracticeEnemyinfoResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_cmt")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiCmt { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_cmt_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiCmtId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_deck")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiDeck ApiDeck { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_deckname")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiDeckname { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_deckname_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiDecknameId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_experience")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiExperience { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_friend")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFriend { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_furniture")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFurniture { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_member_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMemberId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_nickname")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiNickname { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_nickname_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiNicknameId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_rank")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiRank { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiShip { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_slotitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiSlotitem { get; set; } = new();
}
