using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.SpMidnight;

public class ApiReqCombinedBattleSpMidnightRequest : IBattleApiRequest
{
	[JsonPropertyName("api_token")]
	public string ApiToken { get; set; } = string.Empty;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = string.Empty;

	[JsonPropertyName("api_formation")]
	public string ApiFormation { get; set; } = string.Empty;

	[JsonPropertyName("api_recovery_type")]
	public string ApiRecoveryType { get; set; } = string.Empty;

	[JsonPropertyName("api_smoke_flag")]
	public string? ApiSmokeFlag { get; set; }
}
