using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.ChangeDeploymentBase;

public class ApiReqAirCorpsChangeDeploymentBaseResponse
{
	[JsonPropertyName("api_base_items")]
	public List<ApiBaseItem> ApiBaseItems { get; set; } = new();
}
