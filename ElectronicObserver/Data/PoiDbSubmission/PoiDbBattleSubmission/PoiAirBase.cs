using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;

public class PoiAirBase
{
	[JsonPropertyName("api_action_kind")]
	public required AirBaseActionKind ApiActionKind { get; init; }

	[JsonPropertyName("api_area_id")]
	public required int ApiAreaId { get; init; }

	[JsonPropertyName("api_distance")]
	public required ApiDistance ApiDistance { get; init; }

	[JsonPropertyName("api_name")]
	public required string ApiName { get; init; }

	[JsonPropertyName("api_plane_info")]
	public required List<PoiPlaneInfo> ApiPlaneInfo { get; init; }

	[JsonPropertyName("api_rid")]
	public required int ApiRid { get; init; }
}
