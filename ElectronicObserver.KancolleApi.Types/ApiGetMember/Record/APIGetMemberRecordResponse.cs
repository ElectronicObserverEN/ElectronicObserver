using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Record;

public class ApiGetMemberRecordResponse
{
	[JsonPropertyName("api_cmt")]
	[Required(AllowEmptyStrings = true)]
	public string ApiCmt { get; set; } = default!;

	[JsonPropertyName("api_cmt_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiCmtId { get; set; } = default!;

	[JsonPropertyName("api_complate")]
	[Required]
	public List<string> ApiComplate { get; set; } = new();

	[JsonPropertyName("api_deck")]
	public int ApiDeck { get; set; } = default!;

	[JsonPropertyName("api_experience")]
	[Required]
	public List<int> ApiExperience { get; set; } = new();

	[JsonPropertyName("api_friend")]
	public int ApiFriend { get; set; } = default!;

	[JsonPropertyName("api_furniture")]
	public int ApiFurniture { get; set; } = default!;

	[JsonPropertyName("api_kdoc")]
	public int ApiKdoc { get; set; } = default!;

	[JsonPropertyName("api_large_dock")]
	public int ApiLargeDock { get; set; } = default!;

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; } = default!;

	[JsonPropertyName("api_material_max")]
	public int ApiMaterialMax { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; } = default!;

	[JsonPropertyName("api_mission")]
	[Required]
	public ApiMission ApiMission { get; set; } = new();

	[JsonPropertyName("api_ndoc")]
	public int ApiNdoc { get; set; } = default!;

	[JsonPropertyName("api_nickname")]
	[Required(AllowEmptyStrings = true)]
	public string ApiNickname { get; set; } = default!;

	[JsonPropertyName("api_nickname_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiNicknameId { get; set; } = default!;

	[JsonPropertyName("api_photo_url")]
	[Required(AllowEmptyStrings = true)]
	public string ApiPhotoUrl { get; set; } = default!;

	[JsonPropertyName("api_practice")]
	[Required]
	public ApiWar ApiPractice { get; set; } = new();

	[JsonPropertyName("api_rank")]
	public int ApiRank { get; set; } = default!;

	[JsonPropertyName("api_ship")]
	[Required]
	public List<int> ApiShip { get; set; } = new();

	[JsonPropertyName("api_slotitem")]
	[Required]
	public List<int> ApiSlotitem { get; set; } = new();

	[JsonPropertyName("api_war")]
	[Required]
	public ApiWar ApiWar { get; set; } = new();
}
