using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;

public class ApiReqBattleMidnightSpMidnightRequest : IBattleApiRequest
{
	[JsonPropertyName("api_formation")]
	public string ApiFormation { get; set; } = default!;

	[JsonPropertyName("api_recovery_type")]
	public string ApiRecoveryType { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_smoke_flag")]
	public string? ApiSmokeFlag { get; set; }
}
