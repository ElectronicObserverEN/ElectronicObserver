using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Mapinfo;

public class ApiGetMemberMapinfoResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_air_base")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiAirBase> ApiAirBase { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_map_info")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMapInfo> ApiMapInfo { get; set; } = new();
}
