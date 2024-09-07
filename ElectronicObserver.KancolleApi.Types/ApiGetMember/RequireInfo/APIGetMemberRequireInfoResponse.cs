using ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Unsetslot;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.RequireInfo;

public class ApiGetMemberRequireInfoResponse
{
	[JsonPropertyName("api_basic")]
	public ApiBasic ApiBasic { get; set; } = new();

	[JsonPropertyName("api_extra_supply")]
	public List<int> ApiExtraSupply { get; set; } = [];

	[JsonPropertyName("api_furniture")]
	public List<ApiFurniture> ApiFurniture { get; set; } = [];

	[JsonPropertyName("api_kdock")]
	public List<ApiGetMemberKdockResponse> ApiKdock { get; set; } = [];

	[JsonPropertyName("api_oss_setting")]
	public ApiossSetting ApiOssSetting { get; set; } = new();

	[JsonPropertyName("api_position_id")]
	public int? ApiPositionId { get; set; }

	[JsonPropertyName("api_skin_id")]
	public int ApiSkinId { get; set; }

	[JsonPropertyName("api_slot_item")]
	public List<ApiSlotItem> ApiSlotItem { get; set; } = [];

	[JsonPropertyName("api_unsetslot")]
	public ApiGetMemberUnsetslotResponse ApiUnsetslot { get; set; } = [];

	[JsonPropertyName("api_useitem")]
	public List<ApiUseitem> ApiUseitem { get; set; } = [];
}
