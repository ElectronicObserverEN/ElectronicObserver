﻿namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiMaterial
{
	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMemberId { get; set; } = default!;

	[JsonPropertyName("api_value")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiValue { get; set; } = default!;
}
