using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Record;

public class ApiGetMemberRecordResponse
{
	[JsonPropertyName("api_cmt")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiCmt { get; set; } = default!;

	[JsonPropertyName("api_cmt_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiCmtId { get; set; } = default!;

	[JsonPropertyName("api_complate")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<string> ApiComplate { get; set; } = new();

	[JsonPropertyName("api_deck")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiDeck { get; set; } = default!;

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

	[JsonPropertyName("api_kdoc")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiKdoc { get; set; } = default!;

	[JsonPropertyName("api_large_dock")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLargeDock { get; set; } = default!;

	[JsonPropertyName("api_level")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_material_max")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMaterialMax { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMemberId { get; set; } = default!;

	[JsonPropertyName("api_mission")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiMission ApiMission { get; set; } = new();

	[JsonPropertyName("api_ndoc")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNdoc { get; set; } = default!;

	[JsonPropertyName("api_nickname")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiNickname { get; set; } = default!;

	[JsonPropertyName("api_nickname_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiNicknameId { get; set; } = default!;

	[JsonPropertyName("api_photo_url")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiPhotoUrl { get; set; } = default!;

	[JsonPropertyName("api_practice")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiWar ApiPractice { get; set; } = new();

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

	[JsonPropertyName("api_war")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiWar ApiWar { get; set; } = new();
}
