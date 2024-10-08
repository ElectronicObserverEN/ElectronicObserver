﻿using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdShooting;

public class ApiReqCombinedBattleLdShootingRequest : IBattleApiRequest
{
	/// <inheritdoc />
	[JsonPropertyName("api_smoke_flag")]
	public string? ApiSmokeFlag { get; set; }
}
