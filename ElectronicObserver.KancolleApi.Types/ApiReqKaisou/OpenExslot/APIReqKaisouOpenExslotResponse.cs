﻿namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.OpenExslot;

public class ApiReqKaisouOpenExslotResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = default!;
}
