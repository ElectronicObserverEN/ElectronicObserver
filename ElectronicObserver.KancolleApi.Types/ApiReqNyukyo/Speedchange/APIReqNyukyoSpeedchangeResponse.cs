﻿namespace ElectronicObserver.KancolleApi.Types.ApiReqNyukyo.Speedchange;

public class ApiReqNyukyoSpeedchangeResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	[Required(AllowEmptyStrings = true)]
	public string ApiResultMsg { get; set; } = default!;
}
