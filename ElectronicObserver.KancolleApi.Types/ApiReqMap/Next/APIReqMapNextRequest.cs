﻿namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;

public class ApiReqMapNextRequest
{
	[JsonPropertyName("api_cell_id")]
	public string? ApiCellId { get; set; } = default!;

	[JsonPropertyName("api_recovery_type")]
	public string ApiRecoveryType { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
