using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiStage2
{
	[JsonPropertyName("api_air_fire")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiAirFire? ApiAirFire { get; set; } = default!;

	[JsonPropertyName("api_e_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiECount { get; set; } = default!;

	[JsonPropertyName("api_e_lostcount")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiELostcount { get; set; } = default!;

	[JsonPropertyName("api_f_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFCount { get; set; } = default!;

	[JsonPropertyName("api_f_lostcount")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFLostcount { get; set; } = default!;
}
