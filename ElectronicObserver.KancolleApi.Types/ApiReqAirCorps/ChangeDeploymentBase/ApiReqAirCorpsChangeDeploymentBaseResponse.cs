﻿using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqAirCorps.ChangeDeploymentBase;

public class ApiReqAirCorpsChangeDeploymentBaseResponse
{
	[JsonPropertyName("api_base_items")]
	[Required]
	public List<ApiBaseItem> ApiBaseItems { get; set; } = new();
}
