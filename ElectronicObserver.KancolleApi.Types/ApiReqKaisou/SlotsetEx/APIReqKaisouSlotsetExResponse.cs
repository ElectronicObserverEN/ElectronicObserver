﻿namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.SlotsetEx;

public class ApiReqKaisouSlotsetExResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = default!;
}
