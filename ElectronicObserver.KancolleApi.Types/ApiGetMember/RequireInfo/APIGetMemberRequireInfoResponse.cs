using ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.RequireInfo;

public class ApiGetMemberRequireInfoResponse
{
	[JsonPropertyName("api_basic")]
	[Required]
	public ApiBasic ApiBasic { get; set; } = new();

	[JsonPropertyName("api_extra_supply")]
	[Required]
	public List<int> ApiExtraSupply { get; set; } = new();

	[JsonPropertyName("api_furniture")]
	[Required]
	public List<ApiFurniture> ApiFurniture { get; set; } = new();

	[JsonPropertyName("api_kdock")]
	[Required]
	public List<ApiGetMemberKdockResponse> ApiKdock { get; set; } = new();

	[JsonPropertyName("api_oss_setting")]
	[Required]
	public ApiossSetting ApiOssSetting { get; set; } = new();

	[JsonPropertyName("api_position_id")]
	public int? ApiPositionId { get; set; } = default!;

	[JsonPropertyName("api_skin_id")]
	public int ApiSkinId { get; set; } = default!;

	[JsonPropertyName("api_slot_item")]
	[Required]
	public List<ApiSlotItem> ApiSlotItem { get; set; } = new();

	[JsonPropertyName("api_unsetslot")]
	[Required]
	public IDictionary<string, List<int>> ApiUnsetslot { get; set; } = new Dictionary<string, List<int>>();

	[JsonPropertyName("api_useitem")]
	[Required]
	public List<ApiUseitem> ApiUseitem { get; set; } = new();
}
