﻿namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiBasic
{
	[JsonPropertyName("api_firstflag")]
	public int ApiFirstflag { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; } = default!;
}
