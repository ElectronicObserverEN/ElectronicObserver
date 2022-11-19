﻿namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Start;

public class ApiReqQuestStartRequest
{
	[JsonPropertyName("api_quest_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiQuestId { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
