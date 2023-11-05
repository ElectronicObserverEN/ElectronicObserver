using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.Battle;

public class ApiReqPracticeBattleRequest : IBattleApiRequest
{
	[JsonPropertyName("api_deck_id")]
	public string ApiDeckId { get; set; } = "";

	[JsonPropertyName("api_enemy_id")]
	public string ApiEnemyId { get; set; } = "";

	[JsonPropertyName("api_formation_id")]
	public string ApiFormationId { get; set; } = "";

	[JsonPropertyName("api_start")]
	public string? ApiStart { get; set; }

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("api_smoke_flag")]
	public string? ApiSmokeFlag { get; set; }
}
