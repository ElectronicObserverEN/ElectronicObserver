﻿namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Models;

public class ApiPresetSlotItem
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_level")]
	public int ApiLevel { get; set; }
}
