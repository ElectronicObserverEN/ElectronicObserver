using ElectronicObserver.KancolleApi.Types.ApiGetMember.Unsetslot;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyship;

public class ApiReqKousyouDestroyshipResponse
{
	[JsonPropertyName("api_material")]
	public List<int> ApiMaterial { get; set; } = [];

	[JsonPropertyName("api_unset_list")]
	public ApiGetMemberUnsetslotResponse ApiUnsetList { get; set; } = [];
}
