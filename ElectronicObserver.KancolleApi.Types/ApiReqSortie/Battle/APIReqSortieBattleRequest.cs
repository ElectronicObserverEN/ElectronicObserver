using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;

public class ApiReqSortieBattleRequest : IBattleApiRequest
{
	[JsonPropertyName("api_formation")]
	[Required(AllowEmptyStrings = true)]
	public string ApiFormation { get; set; } = default!;

	[JsonPropertyName("api_recovery_type")]
	[Required(AllowEmptyStrings = true)]
	public string ApiRecoveryType { get; set; } = default!;

	[JsonPropertyName("api_start")]
	public string? ApiStart { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_smoke_flag")]
	public string? ApiSmokeFlag { get; set; }
}
