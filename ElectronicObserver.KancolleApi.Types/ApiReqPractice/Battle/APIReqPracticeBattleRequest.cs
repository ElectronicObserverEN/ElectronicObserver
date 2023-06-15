using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.Battle;

public class ApiReqPracticeBattleRequest : IBattleApiRequest
{
	[JsonPropertyName("api_deck_id")]
	public string ApiDeckId { get; set; } = default!;

	[JsonPropertyName("api_enemy_id")]
	public string ApiEnemyId { get; set; } = default!;

	[JsonPropertyName("api_formation_id")]
	public string ApiFormationId { get; set; } = default!;

	[JsonPropertyName("api_start")]
	public string? ApiStart { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_smoke_flag")]
	public string? ApiSmokeFlag { get; set; }
}
