using ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.RequireInfo;

public class ApiGetMemberRequireInfoResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_basic")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiBasic ApiBasic { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_extra_supply")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiExtraSupply { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_furniture")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiFurniture> ApiFurniture { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_kdock")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiGetMemberKdockResponse> ApiKdock { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_oss_setting")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiossSetting ApiOssSetting { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_position_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiPositionId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_skin_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSkinId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_slot_item")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiSlotItem> ApiSlotItem { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_unsetslot")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public IDictionary<string, List<int>> ApiUnsetslot { get; set; } = new Dictionary<string, List<int>>();

	[System.Text.Json.Serialization.JsonPropertyName("api_useitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiUseitem> ApiUseitem { get; set; } = new();
}
