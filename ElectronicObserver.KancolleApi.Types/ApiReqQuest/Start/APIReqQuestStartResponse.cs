﻿namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Start;

public class ApiReqQuestStartResponse
{
	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; } = default!;

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMsg { get; set; } = default!;
}
