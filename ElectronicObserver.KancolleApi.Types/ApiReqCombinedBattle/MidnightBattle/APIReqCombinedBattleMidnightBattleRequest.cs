using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.MidnightBattle;

public class ApiReqCombinedBattleMidnightBattleRequest : IBattleApiRequest
{
	[JsonPropertyName("api_smoke_flag")]
	public string? ApiSmokeFlag { get; set; }
}
