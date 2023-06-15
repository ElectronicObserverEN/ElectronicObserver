﻿using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdAirbattle;

public class ApiReqSortieLdAirbattleRequest : IBattleApiRequest
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
