using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqHokyu.Charge;

public class ApiReqHokyuChargeResponse
{
	[JsonPropertyName("api_material")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiMaterial { get; set; } = new();

	[JsonPropertyName("api_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiChargeShip> ApiShip { get; set; } = new();

	[JsonPropertyName("api_use_bou")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiUseBou { get; set; } = default!;
}
