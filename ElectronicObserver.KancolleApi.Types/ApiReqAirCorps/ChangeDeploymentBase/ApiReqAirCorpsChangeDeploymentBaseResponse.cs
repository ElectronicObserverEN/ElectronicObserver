using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.ChangeDeploymentBase;

public class ApiReqAirCorpsChangeDeploymentBaseResponse
{
	[JsonPropertyName("api_base_items")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiBaseItem> ApiBaseItems { get; set; } = new();
}
