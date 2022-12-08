using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Mapinfo;

public class ApiGetMemberMapinfoResponse
{
	[JsonPropertyName("api_air_base")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiAirBase> ApiAirBase { get; set; } = new();

	[JsonPropertyName("api_map_info")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMapInfo> ApiMapInfo { get; set; } = new();
}
