namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdShooting;

public class ApiReqCombinedBattleLdShootingRequest
{
	[JsonPropertyName("api_token")]
	public required string ApiToken { get; set; }

	[JsonPropertyName("api_verno")]
	public required string ApiVerno { get; set; }

	[JsonPropertyName("api_formation")]
	public required string ApiFormation { get; set; }

	[JsonPropertyName("api_recovery_type")]
	public required string ApiRecoveryType { get; set; }
}
