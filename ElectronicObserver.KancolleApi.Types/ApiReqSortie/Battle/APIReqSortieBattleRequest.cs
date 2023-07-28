﻿using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;

public class ApiReqSortieBattleRequest : IBattleApiRequest
{
	[JsonPropertyName("api_formation")]
	public string ApiFormation { get; set; } = "";

	[JsonPropertyName("api_recovery_type")]
	public string ApiRecoveryType { get; set; } = "";

	[JsonPropertyName("api_start")]
	public string? ApiStart { get; set; }

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	[JsonPropertyName("api_smoke_flag")]
	public string? ApiSmokeFlag { get; set; }
}
