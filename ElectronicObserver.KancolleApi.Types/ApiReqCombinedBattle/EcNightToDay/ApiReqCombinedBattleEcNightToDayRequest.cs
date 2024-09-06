using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcNightToDay;

public class ApiReqCombinedBattleEcNightToDayRequest : IBattleApiRequest
{
	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	/// <inheritdoc />
	[JsonPropertyName("api_smoke_flag")]
	public string? ApiSmokeFlag { get; set; }
}
