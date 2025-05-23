using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;

public class ReplayAirBase
{
	[JsonPropertyName("rid")]
	public int Rid { get; set; }

	[JsonPropertyName("range")]
	public ReplayRange Range { get; set; }

	[JsonPropertyName("action")]
	public AirBaseActionKind Action { get; set; }

	[JsonPropertyName("planes")]
	public List<ReplayAirBaseSquadron> Planes { get; set; }
}
