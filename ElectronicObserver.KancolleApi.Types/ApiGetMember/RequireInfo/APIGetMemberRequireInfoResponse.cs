using ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.RequireInfo;

public class ApiGetMemberRequireInfoResponse
{
	[JsonPropertyName("api_basic")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiBasic ApiBasic { get; set; } = new();

	[JsonPropertyName("api_extra_supply")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiExtraSupply { get; set; } = new();

	[JsonPropertyName("api_furniture")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiFurniture> ApiFurniture { get; set; } = new();

	[JsonPropertyName("api_kdock")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiGetMemberKdockResponse> ApiKdock { get; set; } = new();

	[JsonPropertyName("api_oss_setting")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiossSetting ApiOssSetting { get; set; } = new();

	[JsonPropertyName("api_position_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiPositionId { get; set; } = default!;

	[JsonPropertyName("api_skin_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSkinId { get; set; } = default!;

	[JsonPropertyName("api_slot_item")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiSlotItem> ApiSlotItem { get; set; } = new();

	[JsonPropertyName("api_unsetslot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public IDictionary<string, List<int>> ApiUnsetslot { get; set; } = new Dictionary<string, List<int>>();

	[JsonPropertyName("api_useitem")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiUseitem> ApiUseitem { get; set; } = new();
}
