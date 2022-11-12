using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Record;

public class ApiGetMemberRecordResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_cmt")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiCmt { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_cmt_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiCmtId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_complate")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<string> ApiComplate { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_deck")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiDeck { get; set; } = default!;

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

	[System.Text.Json.Serialization.JsonPropertyName("api_kdoc")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiKdoc { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_large_dock")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLargeDock { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_material_max")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMaterialMax { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_member_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMemberId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_mission")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiMission ApiMission { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ndoc")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiNdoc { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_nickname")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiNickname { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_nickname_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiNicknameId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_photo_url")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiPhotoUrl { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_practice")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiWar ApiPractice { get; set; } = new();

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

	[System.Text.Json.Serialization.JsonPropertyName("api_war")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiWar ApiWar { get; set; } = new();
}
