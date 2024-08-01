using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.AirRaid;

public class HeavyBaseAirRaidWave
{
	[JsonPropertyName("api_formation")]
	public List<int> ApiFormation { get; set; } = [];

	[JsonPropertyName("api_ship_ke")]
	public List<int> ApiShipKe { get; set; } = [];

	[JsonPropertyName("api_ship_lv")]
	public List<int> ApiShipLv { get; set; } = [];

	[JsonPropertyName("api_e_nowhps")]
	public List<int> ApiENowhps { get; set; } = [];

	[JsonPropertyName("api_e_maxhps")]
	public List<int> ApiEMaxhps { get; set; } = [];

	[JsonPropertyName("api_eSlot")]
	public List<List<int>> ApiESlot { get; set; } = [];

	[JsonPropertyName("api_f_nowhps")]
	public List<int> ApiFNowhps { get; set; } = [];

	[JsonPropertyName("api_f_maxhps")]
	public List<int> ApiFMaxhps { get; set; } = [];

	[JsonPropertyName("api_air_base_attack")]
	public ApiAirBaseRaid ApiAirBaseAttack { get; set; } = null!;

	[JsonPropertyName("api_lost_kind")]
	public int ApiLostKind { get; set; }
}
