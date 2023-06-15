using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Record;

public class ApiGetMemberRecordResponse
{
	[JsonPropertyName("api_cmt")]
	public string ApiCmt { get; set; } = "";

	[JsonPropertyName("api_cmt_id")]
	public string ApiCmtId { get; set; } = "";

	[JsonPropertyName("api_complate")]
	public List<string> ApiComplate { get; set; } = new();

	[JsonPropertyName("api_deck")]
	public int ApiDeck { get; set; }

	[JsonPropertyName("api_experience")]
	public List<int> ApiExperience { get; set; } = new();

	[JsonPropertyName("api_friend")]
	public int ApiFriend { get; set; }

	[JsonPropertyName("api_furniture")]
	public int ApiFurniture { get; set; }

	[JsonPropertyName("api_kdoc")]
	public int ApiKdoc { get; set; }

	[JsonPropertyName("api_large_dock")]
	public int ApiLargeDock { get; set; }

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; }

	[JsonPropertyName("api_material_max")]
	public int ApiMaterialMax { get; set; }

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; }

	[JsonPropertyName("api_mission")]
	public ApiMission ApiMission { get; set; } = new();

	[JsonPropertyName("api_ndoc")]
	public int ApiNdoc { get; set; }

	[JsonPropertyName("api_nickname")]
	public string ApiNickname { get; set; } = "";

	[JsonPropertyName("api_nickname_id")]
	public string ApiNicknameId { get; set; } = "";

	[JsonPropertyName("api_photo_url")]
	public string ApiPhotoUrl { get; set; } = "";

	[JsonPropertyName("api_practice")]
	public ApiWar ApiPractice { get; set; } = new();

	[JsonPropertyName("api_rank")]
	public int ApiRank { get; set; }

	[JsonPropertyName("api_ship")]
	public List<int> ApiShip { get; set; } = new();

	[JsonPropertyName("api_slotitem")]
	public List<int> ApiSlotitem { get; set; } = new();

	[JsonPropertyName("api_war")]
	public ApiWar ApiWar { get; set; } = new();
}
