namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.AirRaid;

// todo: no idea if there's supposed to be more data here
public class ApiReqMapAirRaidResponse
{
	[JsonPropertyName("api_destruction_battle")]
	public List<HeavyBaseAirRaidWave> ApiDestructionBattle { get; set; } = [];
}
