﻿using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiStage1
{
	[JsonPropertyName("api_disp_seiku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public AirState ApiDispSeiku { get; set; } = default!;

	[JsonPropertyName("api_e_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiECount { get; set; } = default!;

	[JsonPropertyName("api_e_lostcount")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiELostcount { get; set; } = default!;

	[JsonPropertyName("api_f_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFCount { get; set; } = default!;

	[JsonPropertyName("api_f_lostcount")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFLostcount { get; set; } = default!;

	[JsonPropertyName("api_touch_plane")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<EquipmentId> ApiTouchPlane { get; set; } = new();
}
